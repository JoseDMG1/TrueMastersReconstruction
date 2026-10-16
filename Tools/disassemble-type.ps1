param([string]$TypeName='GameManager')
$ErrorActionPreference='Stop'
Add-Type -TypeDefinition @"
using System; using System.Runtime.InteropServices;
public static class Arm64Decoder {
[DllImport(@"C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\capstone-5.0.6\capstone\lib\capstone.dll",CallingConvention=CallingConvention.Cdecl)]static extern int cs_open(int arch,int mode,out UIntPtr h);
[DllImport(@"C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\capstone-5.0.6\capstone\lib\capstone.dll",CallingConvention=CallingConvention.Cdecl)]static extern UIntPtr cs_disasm(UIntPtr h,byte[] code,UIntPtr len,ulong address,UIntPtr count,out IntPtr result);
[DllImport(@"C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\capstone-5.0.6\capstone\lib\capstone.dll",CallingConvention=CallingConvention.Cdecl)]static extern void cs_free(IntPtr p,UIntPtr count);
[DllImport(@"C:\Users\Jose\Desktop\TrueMastersReconstruction\Tools\capstone-5.0.6\capstone\lib\capstone.dll",CallingConvention=CallingConvention.Cdecl)]static extern int cs_close(ref UIntPtr h);
public static string[] Decode(byte[] bytes,ulong address){UIntPtr h;if(cs_open(1,0,out h)!=0)throw new Exception("cs_open");IntPtr p;UIntPtr n=cs_disasm(h,bytes,(UIntPtr)bytes.Length,address,UIntPtr.Zero,out p);int count=(int)n.ToUInt64();var rows=new string[count];for(int i=0;i<count;i++){IntPtr r=IntPtr.Add(p,i*248);ulong a=(ulong)Marshal.ReadInt64(r,8);string m=Marshal.PtrToStringAnsi(IntPtr.Add(r,42)),o=Marshal.PtrToStringAnsi(IntPtr.Add(r,74));rows[i]="0x"+a.ToString("X")+" "+m+" "+o;}cs_free(p,n);cs_close(ref h);return rows;}
}
"@
$base='C:\Users\Jose\Desktop\TrueMastersReconstruction';$map=Get-Content -LiteralPath "$base\Evidence\cpp2il-native-address-map.json" -Raw | ConvertFrom-Json;$bin=[IO.File]::ReadAllBytes("$base\Evidence\acquired\merged-public-apk\lib\arm64-v8a\libil2cpp.so");$out=New-Object System.Collections.Generic.List[string];foreach($m in $map.entries | Where-Object declaring_type -eq $TypeName){$out.Add("Method: $($m.method_signature)");$out.Add("Native VA: $($m.native_address); raw offset: $($m.raw_file_offset); length: $($m.length)");if($m.raw_file_offset -and $m.length){$offset=[Convert]::ToInt32($m.raw_file_offset.Substring(2),16);$length=[Convert]::ToInt32($m.length.Substring(2),16);$bytes=New-Object byte[] $length;[Array]::Copy($bin,$offset,$bytes,0,$length);foreach($row in [Arm64Decoder]::Decode($bytes,[Convert]::ToUInt64($m.native_address.Substring(2),16))){$out.Add($row)}};$out.Add('')};$safeTypeName=$TypeName.Replace('/','_').Replace('<','_').Replace('>','_');$path="$base\Evidence\native-disassembly-$safeTypeName.txt";[IO.File]::WriteAllLines($path,$out.ToArray());$path

