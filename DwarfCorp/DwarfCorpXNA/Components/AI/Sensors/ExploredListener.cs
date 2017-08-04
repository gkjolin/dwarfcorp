﻿// VoxelListener.cs
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
using System.Runtime.Serialization;
using System.Text;
using DwarfCorp.GameStates;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace DwarfCorp
{
    [JsonObject(IsReference = true)]
    public class ExploredListener : GameComponent
    {
        public LocalVoxelCoordinate VoxelID;

        [JsonIgnore]
        public VoxelChunk Chunk;

        public GlobalChunkCoordinate ChunkID { get; set; }


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Chunk = (context.Context as WorldManager).ChunkManager.ChunkData.GetChunk(ChunkID);
            Chunk.OnVoxelExplored += ExploredListener_OnVoxelExplored;
        }

        public ExploredListener()
        {

        }

        public ExploredListener(ComponentManager manager, ChunkManager chunkManager, VoxelHandle vref) :
            base("ExploredListener", manager)
        {
            Chunk = vref.Chunk;
            VoxelID = vref.Coordinate.GetLocalVoxelCoordinate();
            Chunk.OnVoxelExplored += ExploredListener_OnVoxelExplored;
            ChunkID = Chunk.ID;

        }

        void ExploredListener_OnVoxelExplored(LocalVoxelCoordinate voxelID)
        {
            if (voxelID == VoxelID)
            {
                GetRoot().SetFlagRecursive(Flag.Active, true);
                GetRoot().SetFlagRecursive(Flag.Visible, true);
                Delete();
            }
        }

        public override void Die()
        {
            Chunk.OnVoxelExplored -= ExploredListener_OnVoxelExplored;
            base.Die();
        }

        public override void Delete()
        {
            Chunk.OnVoxelExplored -= ExploredListener_OnVoxelExplored;
            base.Delete();
        }
    }
}
