using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class NeedyAirTrafficController : MonoBehaviour {
    public KMSelectable[] planes;
    public TextMesh[] originText;
    public TextMesh[] destText;
    public Renderer[] planeMats;

    private int[] solution = new int[] { -1, -1, -1 };

    private static Color[] colors =
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.magenta,
        Color.red,
        Color.white,
        Color.yellow,
    };

    private static string[] locations =
    {
        "ATL", "LAX", "ORD", "DFW", "DEN",
        "JFK", "SFO", "LAS", "SEA", "CLT",
        "EWR", "MCO", "PHX", "MIA", "IAH",
        "BOS", "MSP", "DTW", "FLL", "PHL",
        "LGA", "BWI", "SLC", "DCA", "IAD",
        "SAN", "MDW", "TPA", "HNL", "PDX"
    };

    int[][] grid = {
        new int[]{ -1, 2, 0, 0, 2, 0, 1, 1, 1, 1, 0, 2, 0, 1, 1, 0, 1, 2, 0, 0, 1, 0, 1, 2, 1, 2, 2, 2, 1, 2 },
        new int[]{ 1, -1, 2, 0, 1, 0, 2, 2, 2, 0, 0, 2, 1, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 2, 1, 0, 1, 1, 2, 1 },
        new int[]{ 0, 2, -1, 1, 1, 0, 2, 1, 2, 0, 1, 2, 2, 0, 1, 2, 1, 1, 2, 0, 2, 2, 0, 0, 0, 1, 1, 1, 1, 1 },
        new int[]{ 1, 1, 1, -1, 1, 1, 2, 2, 0, 0, 1, 0, 0, 0, 1, 1, 2, 1, 0, 0, 2, 0, 0, 1, 2, 1, 0, 2, 0, 0 },
        new int[]{ 0, 0, 1, 0, -1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 2, 1, 2, 2, 0, 2, 2, 0, 1, 0, 2, 2, 1, 1, 2 },
        new int[]{ 2, 0, 2, 1, 2, -1, 1, 1, 2, 1, 0, 1, 1, 2, 2, 0, 1, 0, 1, 0, 2, 2, 1, 2, 2, 0, 0, 0, 1, 1 },
        new int[]{ 2, 0, 0, 0, 0, 0, -1, 1, 1, 2, 0, 1, 0, 1, 2, 0, 1, 0, 2, 1, 1, 0, 1, 0, 1, 0, 0, 2, 0, 0 },
        new int[]{ 1, 0, 0, 2, 2, 1, 1, -1, 2, 0, 0, 1, 0, 2, 0, 0, 2, 2, 0, 2, 2, 1, 1, 2, 0, 1, 0, 2, 0, 1 },
        new int[]{ 1, 2, 2, 0, 2, 2, 1, 1, -1, 2, 1, 1, 1, 1, 2, 0, 1, 1, 0, 1, 1, 0, 2, 2, 0, 2, 1, 1, 0, 1 },
        new int[]{ 1, 1, 0, 2, 2, 2, 1, 0, 2, -1, 0, 1, 0, 2, 2, 2, 0, 1, 0, 2, 0, 1, 0, 1, 1, 2, 0, 0, 1, 1 },
        new int[]{ 1, 1, 1, 2, 0, 2, 0, 0, 2, 0, -1, 2, 2, 1, 0, 2, 0, 2, 1, 2, 2, 2, 0, 0, 0, 1, 1, 1, 1, 1 },
        new int[]{ 0, 0, 0, 2, 1, 1, 1, 1, 2, 0, 2, -1, 1, 1, 1, 1, 2, 1, 0, 0, 2, 0, 2, 1, 1, 2, 0, 2, 2, 0 },
        new int[]{ 2, 0, 1, 1, 2, 1, 2, 0, 2, 2, 1, 0, -1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 2, 1, 2, 0, 2, 1, 2, 2 },
        new int[]{ 1, 0, 2, 2, 1, 2, 2, 0, 0, 2, 1, 1, 2, -1, 1, 1, 0, 2, 2, 0, 0, 0, 2, 2, 2, 1, 1, 1, 1, 0 },
        new int[]{ 1, 0, 1, 0, 2, 2, 1, 0, 0, 2, 2, 1, 1, 1, -1, 1, 2, 1, 0, 0, 0, 2, 0, 0, 0, 0, 1, 0, 2, 1 },
        new int[]{ 2, 2, 1, 1, 1, 2, 1, 0, 1, 2, 2, 0, 2, 1, 2, -1, 0, 2, 1, 1, 0, 1, 1, 0, 2, 2, 0, 2, 0, 2 },
        new int[]{ 2, 2, 1, 2, 2, 1, 1, 1, 2, 0, 1, 1, 2, 2, 1, 0, -1, 0, 1, 1, 0, 1, 0, 2, 1, 0, 2, 0, 2, 2 },
        new int[]{ 1, 0, 1, 1, 2, 1, 1, 2, 0, 2, 0, 0, 2, 2, 0, 1, 1, -1, 0, 1, 0, 0, 2, 1, 0, 2, 0, 2, 0, 1 },
        new int[]{ 1, 2, 1, 2, 2, 0, 0, 2, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, -1, 1, 2, 1, 0, 1, 1, 2, 0, 2, 1, 2 },
        new int[]{ 1, 1, 2, 0, 1, 2, 1, 0, 2, 0, 2, 1, 0, 2, 2, 0, 0, 1, 0, -1, 2, 2, 2, 1, 2, 1, 0, 1, 0, 0 },
        new int[]{ 1, 0, 0, 2, 0, 1, 2, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, -1, 1, 1, 2, 0, 1, 2, 1, 2, 0 },
        new int[]{ 1, 0, 2, 0, 0, 1, 1, 2, 0, 0, 2, 1, 1, 0, 0, 2, 0, 0, 2, 0, 2, -1, 0, 1, 2, 0, 1, 1, 0, 0 },
        new int[]{ 1, 1, 0, 0, 2, 1, 2, 0, 1, 2, 1, 0, 1, 1, 0, 2, 0, 0, 0, 2, 1, 2, -1, 1, 1, 2, 0, 2, 0, 0 },
        new int[]{ 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 2, 1, 1, 2, 0, 0, 0, 1, 1, 0, 2, 1, 2, -1, 0, 2, 2, 1, 2, 1 },
        new int[]{ 1, 2, 2, 2, 1, 2, 0, 1, 1, 2, 0, 2, 0, 0, 1, 1, 2, 0, 2, 2, 1, 1, 0, 1, -1, 1, 2, 0, 0, 2 },
        new int[]{ 0, 2, 2, 0, 1, 2, 1, 2, 2, 2, 0, 0, 1, 1, 0, 1, 0, 2, 1, 0, 2, 0, 0, 0, 2, -1, 2, 0, 1, 2 },
        new int[]{ 0, 1, 2, 1, 0, 2, 1, 0, 2, 2, 0, 2, 1, 2, 0, 1, 1, 1, 1, 2, 2, 0, 2, 1, 1, 2, -1, 2, 2, 2 },
        new int[]{ 0, 2, 1, 0, 0, 0, 1, 1, 0, 2, 0, 2, 2, 0, 0, 2, 2, 0, 1, 1, 2, 0, 0, 0, 1, 1, 0, -1, 1, 1 },
        new int[]{ 1, 1, 1, 1, 2, 1, 2, 0, 0, 0, 2, 1, 2, 2, 1, 2, 0, 2, 2, 1, 1, 2, 0, 2, 0, 1, 0, 2, -1, 0 },
        new int[]{ 0, 1, 2, 0, 1, 2, 1, 1, 0, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1, 0, 1, 1, 1, 2, 2, 2, 1, -1 }
    };

    private void Awake()
    {
        KMNeedyModule needy = GetComponent<KMNeedyModule>();
        KMBombInfo bomb = GetComponent<KMBombInfo>();
        needy.OnNeedyActivation += OnNeedyActivation;
        needy.OnNeedyDeactivation += OnNeedyDeactivation;
        needy.OnTimerExpired += OnTimerExpired;
        bomb.OnBombExploded += delegate () { bombSolved = true; };
        bomb.OnBombSolved += delegate () { bombSolved = true; };

        planeMats = new Renderer[3];
        for (int i = 0; i < 3; i++)
        {
            planeMats[i] = planes[i].GetComponentInChildren<KMMaterialInfo>().GetComponent<Renderer>();
            planeMats[i].material.color = Color.gray;
            planes[i].OnInteract += planes[i].GetComponent<PlaneState>().OnInteract;
        }
    }

    private void Start()
    {
        
    }

    protected void OnNeedyActivation()
    {
        int[] locs = new int[6];
        for (int i = 0; i < 6; i++)
        {
            locs[i] = UnityEngine.Random.Range(0, locations.Length);
            for (int j = 0; j < i; j++)
            {
                if (locs[j] == locs[i])
                {
                    locs[i] = UnityEngine.Random.Range(0, locations.Length);
                    j = -1;
                }
            }
        }
        List<TextMesh> origins = new List<TextMesh>(originText);
        List<TextMesh> destinations = new List<TextMesh>(destText);
        List<Color> cols = new List<Color>(colors);
        List<int> locList = new List<int>(locs);
        List<int> plns = new List<int>(new int[] { 0, 1, 2 });

        for (int i = 0; i < 3; i++)
        {
            int orgInd = UnityEngine.Random.Range(0, origins.Count);
            int dstInd = UnityEngine.Random.Range(0, destinations.Count);
            int colInd = UnityEngine.Random.Range(0, cols.Count);
            int plnInd = UnityEngine.Random.Range(0, plns.Count);
            int planeInd = plns[plnInd];
            plns.RemoveAt(plnInd);

            int locInd = UnityEngine.Random.Range(0, locList.Count);
            int loc1 = locList[locInd];
            locList.RemoveAt(locInd);
            locInd = UnityEngine.Random.Range(0, locList.Count);
            int loc2 = locList[locInd];
            locList.RemoveAt(locInd);

            TextMesh org = origins[orgInd];
            origins.RemoveAt(orgInd);
            TextMesh dest = destinations[dstInd];
            destinations.RemoveAt(dstInd);
            Color col = cols[colInd];
            cols.RemoveAt(colInd);
            Renderer mat = planeMats[planeInd];

            org.color = col;
            org.text = locations[loc1];
            dest.color = col;
            dest.text = locations[loc2];
            mat.material.color = col;
            solution[planeInd] = grid[loc1][loc2];
            Debug.Log(org.text + " to " + dest.text + " is " + solution[plnInd] + " (" + (solution[planeInd] == 0 ? "DEP" : solution[planeInd] == 2 ? "ARR" : "CRZ") + ")");
        }
        active = true;
    }

    protected void OnNeedyDeactivation()
    {
        
    }

    protected void OnTimerExpired()
    {
        bool gaveStrike = false;
        for (int i = 0; i < 3; i++)
        {
            originText[i].text = "";
            destText[i].text = "";
            planeMats[i].material.color = Color.gray;

            if (planes[i].GetComponent<PlaneState>().state != solution[i])
            {
                Debug.Log("Plane " + i + " was state " + planes[i].GetComponent<PlaneState>().state + " but solution was " + solution[i]);
                if (!gaveStrike) GetComponent<KMNeedyModule>().HandleStrike();
                gaveStrike = true;
            }
        }
        active = false;
    }

    private void createSolution()
    {
        string cat = "int[][] grid = {\n";
        foreach (string orig in locations)
        {
            cat += "new int[]{ ";
            foreach (string dest in locations)
            {
                if (dest == orig) cat += "-1";
                else cat += UnityEngine.Random.Range(0, 3);
                if (dest != "PDX")
                {
                    cat += ", ";
                }
            }
            if (orig != "PDX")
            {
                cat += " },\n";
            }
            else
            {
                cat += " }\n};";
            }
        }
        Debug.Log(cat);
    }

    private void buildManualTable2()
    {
        string cat = "<table>";

        for (int i = 0; i < grid.Length; i++)
        {
            if (i % 6 == 0)
            {
                cat += i > 0 ? "</tr><tr>" : "<tr>";
            }
            cat += "<td><table><tr><th colspan='2'>" + locations[i] + "</th></tr><tr><th>DEP</th><th>ARR</th></tr><tr><td>";

            List<int> dep = new List<int>();
            List<int> arr = new List<int>();
            int[] row = grid[i];

            for (int j = 0; j < row.Length; j++)
            {
                if (j == i) continue;
                if (row[j] == 0) dep.Add(j);
                if (row[j] == 2) arr.Add(j);
            }

            for (int j = 0; j < dep.Count; j++)
            {
                cat += locations[dep[j]];
                if (j < dep.Count - 1) cat += "<br>";
            }

            cat += "</td><td>";

            for (int j = 0; j < arr.Count; j++)
            {
                cat += locations[arr[j]];
                if (j < arr.Count - 1) cat += "<br>";
            }

            cat += "</td></tr></table></td>\n";
        }
        cat += "</tr></table>";
        Debug.Log(cat);
    }

    //twitch plays
    private bool bombSolved = false;
    private bool active = false;
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} set <plane> <angle> [Sets the specified plane to the specified angle] | Valids planes are 1-3 from top to bottom and valid angles are (u)pward, (d)ownward, and (l)evel | Multiple planes may be set, for example: !{0} set 1 upward 3 level";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*set\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 1 || parameters.Length % 2 == 0)
                yield break;
            List<int> usedPlanes = new List<int>();
            for (int i = 1; i < parameters.Length; i++)
            {
                if (i % 2 == 1)
                {
                    int temp = -1;
                    if (!int.TryParse(parameters[i], out temp))
                        yield break;
                    if (temp < 1 || temp > 3)
                        yield break;
                    if (usedPlanes.Contains(temp))
                        yield break;
                    usedPlanes.Add(temp);
                }
                else
                {
                    string[] valids = { "upward", "u", "level", "l", "downward", "d" };
                    if (!valids.Contains(parameters[i].ToLower()))
                        yield break;
                }
            }
            yield return null;
            string[] angles = { "u", "l", "d" };
            for (int i = 1; i < parameters.Length; i+=2)
            {
                while (planes[int.Parse(parameters[i]) - 1].GetComponent<PlaneState>().state != Array.IndexOf(angles, parameters[i + 1].ToLower().Replace("upward", "u").Replace("level", "l").Replace("downward", "d")))
                {
                    planes[int.Parse(parameters[i]) - 1].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    void TwitchHandleForcedSolve()
    {
        //The code is done in a coroutine instead of here so that if the solvebomb command was executed this will just set all planes to their correct angles when it activates and it wont wait for its turn in the queue
        StartCoroutine(DealWithNeedy());
    }

    private IEnumerator DealWithNeedy()
    {
        char[] angles = { 'u', 'l', 'd' };
        while (!bombSolved)
        {
            while (!active) { yield return null; }
            yield return ProcessTwitchCommand("set 1 " + angles[solution[0]] + " 2 " + angles[solution[1]] + " 3 " + angles[solution[2]]);
            while (active) { yield return null; }
        }
    }
}
