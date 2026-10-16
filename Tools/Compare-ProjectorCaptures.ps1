param([Parameter(Mandatory=$true)][string]$Prefix)
$ErrorActionPreference='Stop'
Add-Type -ReferencedAssemblies System.Drawing -TypeDefinition @"
using System; using System.Drawing; using System.Drawing.Imaging; using System.Runtime.InteropServices;
public static class ProjectorPixels {
static byte[] Read(string path,out int width,out int height){using(var bmp=new Bitmap(path)){width=bmp.Width;height=bmp.Height;var data=bmp.LockBits(new Rectangle(0,0,width,height),ImageLockMode.ReadOnly,PixelFormat.Format32bppArgb);try{if(data.Stride!=width*4)throw new Exception("Unexpected image stride");var bytes=new byte[data.Stride*height];Marshal.Copy(data.Scan0,bytes,0,bytes.Length);return bytes;}finally{bmp.UnlockBits(data);}}}
public static int[] Compare(string a,string b){int w,h,w2,h2;var x=Read(a,out w,out h);var y=Read(b,out w2,out h2);if(w!=w2||h!=h2)throw new Exception("Image dimensions differ");int changed=0,significant=0,max=0,brighter=0,darker=0,minx=w,miny=h,maxx=-1,maxy=-1;for(int i=0;i<x.Length;i+=4){int delta=0;bool up=false,down=false;for(int c=0;c<3;c++){delta=Math.Max(delta,Math.Abs(y[i+c]-x[i+c]));up|=y[i+c]>x[i+c]+2;down|=y[i+c]+2<x[i+c];}max=Math.Max(max,delta);if(delta>0)changed++;if(delta>2){significant++;int px=i/4%w,py=i/4/w;minx=Math.Min(minx,px);miny=Math.Min(miny,py);maxx=Math.Max(maxx,px);maxy=Math.Max(maxy,py);}if(up)brighter++;if(down)darker++;}return new[]{w,h,changed,significant,max,brighter,darker,minx,miny,maxx,maxy};}
}
"@
$taskComparisons=@()
foreach($taskPair in @(@('original','disabled'),@('disabled','white-cookie'),@('white-cookie','black-cookie'))){
 $taskA=[IO.Path]::GetFullPath($Prefix+'.projectors-'+$taskPair[0]+'.png'); $taskB=[IO.Path]::GetFullPath($Prefix+'.projectors-'+$taskPair[1]+'.png')
 $taskValues=[ProjectorPixels]::Compare($taskA,$taskB)
 $taskComparisons += [pscustomobject]@{a=$taskA;b=$taskB;aSha256=(Get-FileHash -LiteralPath $taskA).Hash;bSha256=(Get-FileHash -LiteralPath $taskB).Hash;width=$taskValues[0];height=$taskValues[1];changedPixels=$taskValues[2];changedAbove2=$taskValues[3];maxAbsDelta=$taskValues[4];brighterAbove2=$taskValues[5];darkerAbove2=$taskValues[6];significantBBox=@($taskValues[7..10])}
}
$taskJson=[pscustomobject]@{generatedUtc=[DateTime]::UtcNow.ToString('O');limitation='Whole frozen-frame comparison, including UI; RGB tolerance2. Does not establish original shader equivalence.';comparisons=$taskComparisons}|ConvertTo-Json -Depth 6
[IO.File]::WriteAllText([IO.Path]::GetFullPath($Prefix+'.pixel-comparison.json'),$taskJson)
$taskJson
