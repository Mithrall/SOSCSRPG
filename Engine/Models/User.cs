using System.Collections.Generic;

namespace Engine.Models {
    public class User {
        public string UserName;
        public List<Characters> Characters = new List<Characters>();
    }
}
