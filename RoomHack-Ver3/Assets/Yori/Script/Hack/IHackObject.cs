﻿using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
public interface IHackObject
{
    public List<toolTag> canHackToolTag { get; set; }

    public List<ToolEventBase> nowHackEvent { get; set; }

    public bool CanHack { get; set; }

}
