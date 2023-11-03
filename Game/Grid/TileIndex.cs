using UnityEngine;

[System.Serializable]
public struct TileIndex
{
    public int x;
    public int y;
    public int z;

    public TileIndex(int x, int y, int z)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public TileIndex(int x, int z)
    {
        this.x = x; this.z = z; this.y = -x - z;
    }

    public static TileIndex operator +(TileIndex one, TileIndex two)
    {
        return new TileIndex(one.x + two.x, one.y + two.y, one.z + two.z);
    }

    public static TileIndex operator -(TileIndex one, TileIndex two)
    {
        return new TileIndex(one.x - two.x, one.y - two.y, one.z - two.z);
    }

    public static bool operator ==(TileIndex one, TileIndex two)
    {
        if (one.x == two.x && one.y == two.y && one.z == two.z) return true;
        else return false;
    }

    public static bool operator !=(TileIndex one, TileIndex two)
    {
        if (one.x != two.x || one.y != two.y || one.z != two.z) return true;
        else return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        TileIndex o = (TileIndex)obj;
        if ((System.Object)o == null)
            return false;
        return ((x == o.x) && (y == o.y) && (z == o.z));
    }

    public override int GetHashCode()
    {
        return (x.GetHashCode() ^ (y.GetHashCode() + (int)(Mathf.Pow(2, 32) / (1 + Mathf.Sqrt(5)) / 2) + (x.GetHashCode() << 6) + (x.GetHashCode() >> 2)));
    }

    public override string ToString()
    {
        return string.Format("[" + x + "," + y + "," + z + "]");
    }
}
