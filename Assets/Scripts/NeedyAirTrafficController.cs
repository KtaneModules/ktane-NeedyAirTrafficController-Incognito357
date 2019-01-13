using System.Collections;
using System.Collections.Generic;
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
        needy.OnNeedyActivation += OnNeedyActivation;
        needy.OnNeedyDeactivation += OnNeedyDeactivation;
        needy.OnTimerExpired += OnTimerExpired;

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
            locs[i] = Random.Range(0, locations.Length);
            for (int j = 0; j < i; j++)
            {
                if (locs[j] == locs[i])
                {
                    locs[i] = Random.Range(0, locations.Length);
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
            int orgInd = Random.Range(0, origins.Count);
            int dstInd = Random.Range(0, destinations.Count);
            int colInd = Random.Range(0, cols.Count);
            int plnInd = Random.Range(0, plns.Count);
            int planeInd = plns[plnInd];
            plns.RemoveAt(plnInd);

            int locInd = Random.Range(0, locList.Count);
            int loc1 = locList[locInd];
            locList.RemoveAt(locInd);
            locInd = Random.Range(0, locList.Count);
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
                else cat += Random.Range(0, 3);
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
}
