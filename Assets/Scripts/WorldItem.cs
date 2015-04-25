using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


    public class WorldItem
    {
        public string name { get; set; }
        public int index { get; set; }
        public List<int> linkList { get;set;}

        public Vector3 position { get; set; }

    }

