using System;
using UnityEngine;

public struct TileCoord
{
    public int x;
    public int y;

    public TileCoord(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public static bool operator== (TileCoord l, TileCoord r)
    {

        return l.x == r.x && l.y == r.y;
    }

    public static bool operator!= (TileCoord l, TileCoord r)
    {
        return !(l == r);
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;

        Vector3 v3 = (Vector3) obj;

        if ( (System.Object) v3 == null )
            return false;

        return x == v3.x && y == v3.y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();     
    }

    public static implicit operator TileCoord (Vector3 r)
    {
        return new TileCoord((int) r.x, (int) r.y);
    }

    public static implicit operator TileCoord (Vector2 r)
    {
        return new TileCoord((int) r.x, (int) r.y);
    }

    public static TileCoord operator+ (TileCoord l, Vector3 r)
    {
        float x = l.x + r.x;
        float y = l.y + r.y;

        return new TileCoord((int) x, (int) y);
    }

    public static TileCoord operator- (TileCoord l, Vector3 r)
    {
        float x = l.x - r.x;
        float y = l.y - r.y;

        return new TileCoord((int) x, (int) y);
    }
}