using System;

[Serializable]
public class TMPlayerCharacters
{
	public int aura;

	public int eliot;

	public int ren;

	public int rina;

	public int dante;

	public int agata;

	public int raven;

	public int maya;

	public TMPlayerCharacters()
	{
	}

	public TMPlayerCharacters(int aura, int eliot, int ren, int rina, int dante, int agata, int raven, int maya)
	{
		this.aura = aura; this.eliot = eliot; this.ren = ren; this.rina = rina;
		this.dante = dante; this.agata = agata; this.raven = raven; this.maya = maya;
	}
}
