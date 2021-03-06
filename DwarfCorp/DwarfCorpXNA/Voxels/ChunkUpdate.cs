// ChunkUpdate.cs
// 
//  Modified MIT License (MIT)
//  
//  Copyright (c) 2015 Completely Fair Games Ltd.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// The following content pieces are considered PROPRIETARY and may not be used
// in any derivative works, commercial or non commercial, without explicit 
// written permission from Completely Fair Games:
// 
// * Images (sprites, textures, etc.)
// * 3D Models
// * Sound Effects
// * Music
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;
using System.Threading;

namespace DwarfCorp
{
    public class ChunkUpdate
    {
        private static int CurrentUpdateChunk = 0;

        public static void RunUpdate(ChunkManager Chunks)
        {
            var chunk = Chunks.ChunkData.ChunkMap[CurrentUpdateChunk];
            CurrentUpdateChunk += 1;
            if (CurrentUpdateChunk >= Chunks.ChunkData.ChunkMap.Length)
                CurrentUpdateChunk = 0;

            UpdateChunk(chunk);
        }

        private static void UpdateChunk(VoxelChunk chunk)
        {
            for (var y = 0; y < VoxelConstants.ChunkSizeY; ++y)
            {
                // Skip empty slices.
                if (chunk.Data.VoxelsPresentInSlice[y] == 0) continue;

                for (var x = 0; x < VoxelConstants.ChunkSizeX; ++x)
                    for (var z = 0; z < VoxelConstants.ChunkSizeZ; ++z)
                    {
                        var voxel = new VoxelHandle(chunk, new LocalVoxelCoordinate(x, y, z));

                        if (voxel.Decal != 0)
                        {
                            var decal = DecalLibrary.GetDecalType(voxel.Decal);
                            if (decal.Decay)
                            {
                                voxel.DecalData -= 1;
                                if (voxel.DecalData == 0)
                                {
                                    var newDecal = DecalLibrary.GetDecalType(decal.BecomeWhenDecays);
                                    if (newDecal != null)
                                        voxel.Decal = newDecal.ID;
                                    else
                                        voxel.Decal = 0;
                                }
                            } 
                        }
                    }
            }
        }
    }
}
