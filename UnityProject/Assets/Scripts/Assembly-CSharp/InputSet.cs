using System;
[Serializable]
public class InputSet
{
 public float x;
 public float y;
 public bool a;
 public bool b;
 public bool s;
 public bool Equals(InputSet other) => other != null && x == other.x && y == other.y && a == other.a && b == other.b && s == other.s;
 public InputSet Clone() => new InputSet { x = x, y = y, a = a, b = b, s = s };
}
