global using System;
global using StereoKit;

SKSettings settings = new SKSettings
{
    appName = "Splunky",
    assetsFolder = "add",
    displayPreference = DisplayMode.Flatscreen
};
if (!SK.Initialize(settings))
    Environment.Exit(1);

// Renderer.EnableSky = false;
// Renderer.ClearColor = Color.Hex(0xa9c6e2ff);
// Renderer.SetClip(0.08f, 256f);

SK.Run(() =>
{

});


float Clamp(float value, float min, float max)
{
    return Math.Max(min, Math.Min(value, max));
}


Vec3Int[] dirs = new Vec3Int[] {
    new Vec3Int(-1, 0, 0), new Vec3Int(0, -1, 0), new Vec3Int(0, 0, -1),
    new Vec3Int(1, 0, 0), new Vec3Int(0, 1, 0), new Vec3Int(0, 0, 1)
  };


Vec3Int[] allDirs = new Vec3Int[] {
    new Vec3Int(-1, 0, 0),
    new Vec3Int(0, -1, 0),
    new Vec3Int(0, 0, -1),
    new Vec3Int(-1, -1, 0),
    new Vec3Int(0, -1, -1),
    new Vec3Int(-1, 0, -1),

    new Vec3Int(1, 0, 0),
    new Vec3Int(0, 1, 0),
    new Vec3Int(0, 0, 1),
    new Vec3Int(1, 1, 0),
    new Vec3Int(0, 1, 1),
    new Vec3Int(1, 0, 1),

    new Vec3Int(-1, 1, 0),
    new Vec3Int(0, -1, 1),
    new Vec3Int(1, 0, -1),
    new Vec3Int(1, -1, 0),
    new Vec3Int(0, 1, -1),
    new Vec3Int(-1, 0, 1)
  };

bool Outside(Vec3Int pos)
{
    for (int v = 0; v < voxels.Length; v++)
    {
        if (pos == voxels[v].pos) { return false; }
    }
    return true;
}


void VoxelCollision(VoxelBody vbody)
{
    Vec3 toPos = vbody.pos + vbody.vel * Time.Elapsedf;

    int w = 0;
    while (w < 3)
    {
        Vec3 clampPos = new Vec3(
            Clamp(toPos.x, Bound(vbody, 0, -1), Bound(vbody, 0, 1)),
            Clamp(toPos.y, Bound(vbody, 1, -1), Bound(vbody, 1, 1)),
            Clamp(toPos.z, Bound(vbody, 2, -1), Bound(vbody, 2, 1))
        );

        float largest = 0;
        int largeIndex = -1;
        for (int j = 0; j < 3; j++)
        {
            float dist = MathF.Abs(toPos[j] - clampPos[j]);
            if (dist > largest)
            {
                largeIndex = j;
                largest = dist;
            }
        }

        if (largeIndex > -1)
        {
            toPos[largeIndex] = clampPos[largeIndex];
            vbody.vel[largeIndex] *= -0.25f; // Bounce
        }
        else
        {
            break;
        }

        w++;
    }

    vbody.pos = toPos;
}


float Bound(VoxelBody vbody, int axis, int dir)
{
    Vec3Int step = new Vec3Int(0, 0, 0);
    step[axis] = dir;

    float bound = float.MaxValue * dir;
    float closest = float.MaxValue;
    for (int i = 0; i < mono.allDirs.Length; i++)
    {
        Vec3 d = (Vec3)mono.allDirs[i] * (vbody.boundRadius - 0.001f);
        d[axis] = 0;
        Vec3 vPos = Voxelcast(mono.VoxelPos(vbody.pos + d), step);
        float dist = MathF.Abs(vPos[axis] - vbody.pos[axis]);
        if (dist < closest)
        {
            bound = vPos[axis];
            closest = dist;
        }
    }
    // when hit ?
    return bound + (((1 - vbody.voxelBody.boundRadius * 2) / 2) * dir);
}

Vec3Int Voxelcast(Vec3Int from, Vec3Int step)
{
    Vec3Int vPos = from;
    int i = 0;
    while (i < 15)
    {
        vPos += step;
        if (!mono.InVoxel(vPos))
        {
            vPos -= step;
            break;
        }

        i++;
    }

    return vPos;
}

public class Vec3Int
{
    public int x, y, z;
    public Vec3Int(int x, int y, int z)
    {
        this.x = x; this.y = y; this.z = z;
    }
}

public class VoxelBody
{
    public float boundRadius;
    public Vec3 pos, vel;
    // public float mass;
}

/*

*/
