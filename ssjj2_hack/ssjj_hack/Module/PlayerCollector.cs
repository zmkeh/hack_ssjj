using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ssjj_hack
{
    public class PlayerCollector : ModuleBase
    {
        public override void OnGUI()
        {
            if (GUILayout.Button("Collect"))
            {
                Collect();
                foreach (var p in players)
                {
                    Log.Print(p.name + $" [{p.teamId}]");
                }
            }
        }

        private void Collect()
        {
            var playerScriptType = GetTypeMain("Core.CharacterController.PlayerScript");
            var scripts = Object.FindObjectsOfType(playerScriptType);
            foreach (var p in scripts)
            {
                var comp = p as Component;
                if (!players.Any(a => a.gameObject == comp.gameObject))
                {
                    players.Add(new Player(comp.gameObject));
                }
            }

            for (int i = players.Count - 1; i >= 0; i--)
            {
                if (!players[i].isValid)
                {
                    players.RemoveAt(i);
                }
            }

            foreach (var p in players)
            {
                p.Update();
            }
        }

        private float interval = 1;
        public override void FixedUpdate()
        {
            interval = Mathf.Max(interval - Time.deltaTime, 0);
            if (interval <= 0)
            {
                interval = 1;
                Collect();
            }
        }

        public List<Player> players = new List<Player>();
    }

    public class Player
    {
        public static Player mainPlayer;

        public GameObject gameObject;
        public Transform modelRoot;
        public string name;
        public long teamId;

        public PlayerModel model = new PlayerModel();
        public float hp;
        public float hpMax;

        public bool isMainPlayer => this == mainPlayer;
        public bool isFriend => mainPlayer != null && mainPlayer.teamId == teamId;
        public bool isValid => gameObject != null && model.isValid;
        public float hpRatio => hpMax == 0 ? 0 : hp / hpMax;

        private Component script;
        private Component vehicle;
        private Component raycast;
        private object entity;
        private object info;

        public Player(GameObject gameObject)
        {
            this.gameObject = gameObject;
            Init();
            Update();
        }

        private void Init()
        {
            foreach (var comp in gameObject.GetComponents<Component>())
            {
                var typeName = comp.GetType().Name;
                if (typeName == "PlayerScript")
                    script = comp;
                else if (typeName == "PlayerVehicleCollision")
                    vehicle = comp;
                else if (typeName == "RayCastTarget")
                    raycast = comp;
            }

            if (raycast == null)
            {
                mainPlayer = this;
            }
        }

        public void Update()
        {
            entity = vehicle.ReflectMember("_playerEntity");
            info = entity.ReflectMember("playerInfo");
            modelRoot = gameObject.transform.FindChildDeep("Bip01").parent;
            model.CacheBones(modelRoot);
            name = info.ReflectMember("_PlayerName").ReflectMember("m_InnerString") as string;
            teamId = (long)info.ReflectMember("TeamId");
        }
    }


    public class PlayerModel
    {
        public Transform root;

        public Transform spine;
        public Transform neck;
        public Transform u_head;
        public Transform d_head;

        public Transform l_clavicle;
        public Transform r_clavicle;
        public Transform l_upperarm;
        public Transform r_upperarm;
        public Transform l_forearm;
        public Transform r_forearm;
        public Transform l_hand;
        public Transform r_hand;

        public Transform pelvis;
        public Transform l_thigh;
        public Transform r_thigh;
        public Transform l_calf;
        public Transform r_calf;
        public Transform l_foot;
        public Transform r_foot;

        public bool isValid => isBonesCached && root != null;
        public bool isBonesCached;

        public void CacheBones(Transform root)
        {
            this.root = root;
            spine = root.FindChildDeep("Bip01 Spine");
            neck = root.FindChildDeep("Bip01 Neck");
            d_head = root.FindChildDeep("Bip01 Head");
            u_head = d_head.FindChildDeep("Bip01 HeadNub");

            l_clavicle = root.FindChildDeep("Bip01 L Clavicle");
            r_clavicle = root.FindChildDeep("Bip01 R Clavicle");
            l_upperarm = root.FindChildDeep("Bip01 L UpperArm");
            r_upperarm = root.FindChildDeep("Bip01 R UpperArm");
            l_forearm = root.FindChildDeep("Bip01 L Forearm");
            r_forearm = root.FindChildDeep("Bip01 R Forearm");
            l_hand = root.FindChildDeep("Bip01 L Hand");
            r_hand = root.FindChildDeep("Bip01 R Hand");

            pelvis = root.FindChildDeep("Bip01 Pelvis");
            l_thigh = root.FindChildDeep("Bip01 L Thigh");
            r_thigh = root.FindChildDeep("Bip01 R Thigh");
            l_calf = root.FindChildDeep("Bip01 L Calf");
            r_calf = root.FindChildDeep("Bip01 R Calf");
            l_foot = root.FindChildDeep("Bip01 L Foot");
            r_foot = root.FindChildDeep("Bip01 R Foot");

            if (spine == null || neck == null
                || d_head == null || u_head == null
                || l_clavicle == null || r_clavicle == null
                || l_upperarm == null || r_upperarm == null
                || l_forearm == null || r_forearm == null
                || l_hand == null || r_hand == null
                || pelvis == null
                || l_thigh == null || r_thigh == null
                || l_calf == null || r_calf == null
                || l_foot == null || r_foot == null)
            {
                isBonesCached = false;
            }
            isBonesCached = true;
        }

        public TRect GetRect()
        {
            if (!isValid) return default;
            var p1 = root.GetUIPos();
            var p2 = u_head.GetUIPos();
            if (p1.z <= 0 || p2.z <= 0)
                return new TRect();
            var x = (p1.x + p2.x) * 0.5f;
            var y = (p1.y + p2.y) * 0.5f;
            var h = p2.y - p1.y;
            var w = h * 0.4f;
            return new TRect(x, y, w, h);
        }

        private void AddPoint(List<TCircle> lst, Transform t)
        {
            var p = t.GetUIPos();
            if (p.z <= 0)
                return;
            lst.Add(new TCircle(p, 1));
        }

        public List<TCircle> GetPoints()
        {
            var points = new List<TCircle>();
            if (!isValid) return points;
            AddPoint(points, spine);
            AddPoint(points, neck);
            AddPoint(points, d_head);
            AddPoint(points, u_head);
            AddPoint(points, l_clavicle);
            AddPoint(points, r_clavicle);
            AddPoint(points, l_upperarm);
            AddPoint(points, r_upperarm);
            AddPoint(points, r_forearm);
            AddPoint(points, r_forearm);
            AddPoint(points, l_hand);
            AddPoint(points, r_hand);
            AddPoint(points, pelvis);
            AddPoint(points, l_thigh);
            AddPoint(points, r_thigh);
            AddPoint(points, l_calf);
            AddPoint(points, r_calf);
            AddPoint(points, l_foot);
            AddPoint(points, r_foot);
            return points;
        }

        private void AddLine(List<TLine> lst, Transform t1, Transform t2)
        {
            var p1 = t1.GetUIPos();
            var p2 = t2.GetUIPos();
            if (p1.z <= 0 || p2.z <= 0)
                return;
            lst.Add(new TLine(p1, p2));
        }

        public List<TLine> GetLines()
        {
            var lines = new List<TLine>();
            if (!isValid) return lines;
            AddLine(lines, pelvis, spine);
            AddLine(lines, spine, neck);
            AddLine(lines, neck, l_clavicle);
            AddLine(lines, neck, r_clavicle);
            AddLine(lines, neck, d_head);
            AddLine(lines, d_head, u_head);

            AddLine(lines, l_clavicle, l_upperarm);
            AddLine(lines, r_clavicle, r_upperarm);
            AddLine(lines, l_upperarm, l_forearm);
            AddLine(lines, r_upperarm, r_forearm);
            AddLine(lines, l_forearm, l_hand);
            AddLine(lines, r_forearm, r_hand);

            AddLine(lines, pelvis, l_thigh);
            AddLine(lines, pelvis, r_thigh);
            AddLine(lines, l_thigh, l_calf);
            AddLine(lines, r_thigh, r_calf);
            AddLine(lines, l_calf, l_foot);
            AddLine(lines, r_calf, r_foot);
            return lines;
        }
    }
}
