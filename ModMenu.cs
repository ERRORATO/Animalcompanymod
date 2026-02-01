using UnityEngine;
using System.Collections.Generic;

namespace AnimalCompanyMod {
    public class ErroratoGoofyMenu : MonoBehaviour {
        // --- VERSION LOCK & CONFIG ---
        private string requiredVersion = "1.58.1.2260"; 
        private bool isOutdated = false;
        private bool isVisible = false;
        private Rect windowRect = new Rect(20, 20, 320, 580);
        private int currentPage = 0;
        private const int TotalPages = 5;

        // --- FEATURES STATE ---
        private string coinInput = "69420";
        private float playerScale = 1.0f;
        private bool espActive = false;
        private List<string> combo = new List<string>();

        // Monster List (Prefab Names)
        private string[] goofyMonsters = { 
            "Lanky", "Puppet", "Worm", "AnglerFish", "Crawler", "Banshee", "Ogre", 
            "Employee", "Turkey", "Chicken", "MimicChest", "RedLightMonster", "Segway", "GiantSpider" 
        };

        void Start() {
            // Version Guard check on startup
            if (Application.version != requiredVersion) {
                isOutdated = true;
                isVisible = true; // Force error screen
            }
        }

        void Update() {
            // Combo Logic: Y, B, Y, B
            if (OVRInput.GetDown(OVRInput.RawButton.Y)) HandleCombo("Y");
            if (OVRInput.GetDown(OVRInput.RawButton.B)) HandleCombo("B");

            if (isVisible && !isOutdated) {
                // Attach to hand logic or world space
                transform.localScale = Vector3.one * playerScale;
            }
        }

        void OnGUI() {
            if (!isVisible) return;
            GUI.backgroundColor = Color.black;
            windowRect = GUI.Window(0, windowRect, DrawMasterUI, "ERRORATO'S GOOFY AHH MENU");
        }

        void DrawMasterUI(int id) {
            GUI.contentColor = Color.green;

            if (isOutdated) {
                GUI.color = Color.red;
                GUI.Label(new Rect(10, 50, 280, 200), "!!! UPDATE REQUIRED !!!\n\nMod is for: " + requiredVersion + "\nGame is: " + Application.version + "\n\nUpdate your game and rebuild the .dll!");
                if (GUI.Button(new Rect(10, 250, 280, 50), "QUIT GAME")) Application.Quit();
                return;
            }

            // Page Navigation
            switch (currentPage) {
                case 0: DrawPage1(); break; // Currency
                case 1: DrawPage2(); break; // Combat
                case 2: DrawPage3(); break; // Movement
                case 3: DrawPage4(); break; // Spawner
                case 4: DrawPage5(); break; // World
            }

            // Nav Arrows
            if (GUI.Button(new Rect(10, 530, 80, 35), "< PREV")) currentPage = (currentPage > 0) ? currentPage - 1 : TotalPages - 1;
            GUI.Label(new Rect(135, 535, 60, 20), (currentPage + 1) + "/" + TotalPages);
            if (GUI.Button(new Rect(230, 530, 80, 35), "NEXT >")) currentPage = (currentPage < TotalPages - 1) ? currentPage + 1 : 0;
            GUI.DragWindow();
        }

        // --- PAGES ---
        void DrawPage1() { // Currency
            GUI.Label(new Rect(10, 40, 280, 20), "Enter Amount:");
            coinInput = GUI.TextField(new Rect(10, 65, 280, 30), coinInput);
            if (GUI.Button(new Rect(10, 105, 280, 45), "GIVE NUTS/CC TO ALL (GLOBAL)")) { /* RPC Logic */ }
            if (GUI.Button(new Rect(10, 160, 280, 45), "GIVE +500 RP (GLOBAL)")) { /* RP Logic */ }
            if (GUI.Button(new Rect(10, 215, 280, 45), "UNLOCK ALL BUNDLES")) { /* Unlock Logic */ }
        }

        void DrawPage2() { // Combat
            if (GUI.Button(new Rect(10, 50, 280, 45), "KILL ALL (Mimic/Turkey/RLGL)")) {
                foreach (string m in goofyMonsters) {
                    GameObject[] targets = GameObject.FindObjectsOfType<GameObject>();
                    foreach (GameObject obj in targets) if (obj.name.Contains(m)) Destroy(obj);
                }
            }
            if (GUI.Button(new Rect(10, 110, 280, 45), "RAINBOW MONSTERS")) { /* Rainbow Logic */ }
        }

        void DrawPage3() { // Movement
            GUI.Label(new Rect(10, 50, 280, 20), "Body Scale: " + playerScale + "x");
            playerScale = GUI.HorizontalSlider(new Rect(10, 75, 280, 30), playerScale, 0.1f, 5.0f);
            if (GUI.Button(new Rect(10, 115, 280, 45), "GHOST MODE (NO-CLIP)")) { /* Ghost Logic */ }
            if (GUI.Button(new Rect(10, 170, 280, 45), "LONG ARMS")) { /* Arm Logic */ }
        }

        void DrawPage4() { // Spawner
            string[] items = { "Skateboard", "Scooter", "Trampoline", "Megaphone", "Broccoli Bomb" };
            for(int i=0; i<items.Length; i++) {
                if (GUI.Button(new Rect(10, 50 + (50 * i), 280, 40), "Spawn " + items[i])) { /* Spawn Logic */ }
            }
        }

        void DrawPage5() { // World
            espActive = GUI.Toggle(new Rect(10, 50, 280, 30), espActive, " ENABLE PLAYER ESP");
            if (GUI.Button(new Rect(10, 90, 280, 45), "TP TO SECRET SHOP")) { transform.position = new Vector3(100, 5, 200); }
            if (GUI.Button(new Rect(10, 145, 280, 45), "LOAD DEV SANDBOX")) { /* Map Logic */ }
        }

        void HandleCombo(string key) {
            combo.Add(key);
            if (combo.Count > 4) combo.RemoveAt(0);
            if (string.Join("", combo) == "YBYB") { isVisible = !isVisible; combo.Clear(); }
        }
    }
}

