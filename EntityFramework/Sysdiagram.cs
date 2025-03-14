using System;
using System.Collections.Generic;

#nullable disable

namespace KOAHome.EntityFramework
{
    public partial class Sysdiagram
    {
        public int DiagramId { get; set; }
        public string Name { get; set; }
        public int PrincipalId { get; set; }
        public int? Version { get; set; }
        public byte[] Definition { get; set; }
    }
}
