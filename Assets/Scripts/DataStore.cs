using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Database
{
    private static readonly object threadLock = new object();
    public static Database db;
    public int level = 1;
    public int expLevel = 0;
    public int exp = 0;
    public int slotNum = 1;
    public Dictionary<string, object>[] hotbarItem = {
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "O"},
            {"Quantity", 5}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Mg"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "He"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Na"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Cm"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "Og"},
            {"Quantity", 1}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null
    };
    public Dictionary<string, object>[] flaskItem = {
        null,
        null,
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        new Dictionary<string, object>()
        {
            {"Item", "H"},
            {"Quantity", 64}
        },
        null,
        null,
        null,
        null
    };

    public static void Load()
    {
        string directory = $"{Application.persistentDataPath}/Path Of Chemistry/Data";
        string filePath = $"{directory}/Saves.json";
        lock (threadLock)
        {
            if (db == null)
            {
                db = new Database();
            }
        }
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(filePath))
        {
            Save();
        }
        string fileContent = File.ReadAllText(filePath);
        Database data = JsonConvert.DeserializeObject<Database>(fileContent);
        db = data;
    }

    public static void Save()
    {
        string filePath = $"{Application.persistentDataPath}/Path Of Chemistry/Data/Saves.json";
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string data = JsonConvert.SerializeObject(db, settings);
        File.WriteAllText(filePath, data);
    }

    public static void SaveAndQuit()
    {
        Save();
        db = null;
    }

    public static string Log(object rawData)
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string newData = JsonConvert.SerializeObject(rawData, settings);
        return newData;
    }
}

public static class ReadOnly
{
    public static readonly Dictionary<string, object>[] elements = {
        new Dictionary<string, object>()
        {
            {"symbol", "H"},
            {"name", "Hydrogen"},
            {"group", "Other Non-Metal"},
            {"protons", 1},
            {"electrons", 1},
            {"neutrons", 0},
            {"weight", 1}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "He"},
            {"name", "Helium"},
            {"group", "Noble Gases"},
            {"protons", 2},
            {"electrons", 2},
            {"neutrons", 1},
            {"weight", 4}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Li"},
            {"name", "Lithium"},
            {"group", "Alkali Metals"},
            {"protons", 3},
            {"electrons", 3},
            {"neutrons", 3},
            {"weight", 7}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Be"},
            {"name", "Beryllium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 4},
            {"electrons", 4},
            {"neutrons", 5},
            {"weight", 9}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "B"},
            {"name", "Boron"},
            {"group", "Metalloids"},
            {"protons", 5},
            {"electrons", 5},
            {"neutrons", 5},
            {"weight", 11}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "C"},
            {"name", "Carbon"},
            {"group", "Other Non-Metal"},
            {"protons", 6},
            {"electrons", 6},
            {"neutrons", 5},
            {"weight", 12}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "N"},
            {"name", "Nitrogen"},
            {"group", "Other Non-Metal"},
            {"protons", 7},
            {"electrons", 7},
            {"neutrons", 7},
            {"weight", 14}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "O"},
            {"name", "Oxygen"},
            {"group", "Other Non-Metal"},
            {"protons", 8},
            {"electrons", 8},
            {"neutrons", 8},
            {"weight", 16}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "F"},
            {"name", "Fluorine"},
            {"group", "Halogens"},
            {"protons", 9},
            {"electrons", 9},
            {"neutrons", 9},
            {"weight", 19}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ne"},
            {"name", "Neon"},
            {"group", "Noble Gases"},
            {"protons", 10},
            {"electrons", 10},
            {"neutrons", 10},
            {"weight", 20}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Na"},
            {"name", "Sodium"},
            {"group", "Alkali Metals"},
            {"protons", 11},
            {"electrons", 11},
            {"neutrons", 11},
            {"weight", 23}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Mg"},
            {"name", "Magnesium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 12},
            {"electrons", 12},
            {"neutrons", 12},
            {"weight", 24}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Al"},
            {"name", "Aluminum"},
            {"group", "Post Transition Metals"},
            {"protons", 13},
            {"electrons", 13},
            {"neutrons", 14},
            {"weight", 27}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Si"},
            {"name", "Silicon"},
            {"group", "Metalloids"},
            {"protons", 14},
            {"electrons", 14},
            {"neutrons", 14},
            {"weight", 28}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "P"},
            {"name", "Phosphorus"},
            {"group", "Other Non-Metal"},
            {"protons", 15},
            {"electrons", 15},
            {"neutrons", 16},
            {"weight", 31}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "S"},
            {"name", "Sulfur"},
            {"group", "Other Non-Metal"},
            {"protons", 16},
            {"electrons", 16},
            {"neutrons", 16},
            {"weight", 32}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cl"},
            {"name", "Chlorine"},
            {"group", "Halogens"},
            {"protons", 17},
            {"electrons", 17},
            {"neutrons", 18},
            {"weight", 35}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ar"},
            {"name", "Argon"},
            {"group", "Noble Gases"},
            {"protons", 18},
            {"electrons", 18},
            {"neutrons", 18},
            {"weight", 40}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "K"},
            {"name", "Potassium"},
            {"group", "Alkali Metals"},
            {"protons", 19},
            {"electrons", 19},
            {"neutrons", 20},
            {"weight", 39}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ca"},
            {"name", "Calcium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 20},
            {"electrons", 20},
            {"neutrons", 20},
            {"weight", 40}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sc"},
            {"name", "Scandium"},
            {"group", "Transition Metals"},
            {"protons", 21},
            {"electrons", 21},
            {"neutrons", 24},
            {"weight", 45}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ti"},
            {"name", "Titanium"},
            {"group", "Transition Metals"},
            {"protons", 22},
            {"electrons", 22},
            {"neutrons", 24},
            {"weight", 48}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "V"},
            {"name", "Vanadium"},
            {"group", "Transition Metals"},
            {"protons", 23},
            {"electrons", 23},
            {"neutrons", 27},
            {"weight", 51}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cr"},
            {"name", "Chromium"},
            {"group", "Transition Metals"},
            {"protons", 24},
            {"electrons", 24},
            {"neutrons", 26},
            {"weight", 52}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Mn"},
            {"name", "Manganese"},
            {"group", "Transition Metals"},
            {"protons", 25},
            {"electrons", 25},
            {"neutrons", 29},
            {"weight", 55}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Fe"},
            {"name", "Iron"},
            {"group", "Transition Metals"},
            {"protons", 26},
            {"electrons", 26},
            {"neutrons", 26},
            {"weight", 56}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Co"},
            {"name", "Cobalt"},
            {"group", "Transition Metals"},
            {"protons", 27},
            {"electrons", 27},
            {"neutrons", 30},
            {"weight", 59}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ni"},
            {"name", "Nickel"},
            {"group", "Transition Metals"},
            {"protons", 28},
            {"electrons", 28},
            {"neutrons", 30},
            {"weight", 59}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cu"},
            {"name", "Copper"},
            {"group", "Transition Metals"},
            {"protons", 29},
            {"electrons", 29},
            {"neutrons", 34},
            {"weight", 64}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Zn"},
            {"name", "Zinc"},
            {"group", "Transition Metals"},
            {"protons", 30},
            {"electrons", 30},
            {"neutrons", 34},
            {"weight", 65}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ga"},
            {"name", "Gallium"},
            {"group", "Post Transition Metals"},
            {"protons", 31},
            {"electrons", 31},
            {"neutrons", 36},
            {"weight", 70}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ge"},
            {"name", "Germanium"},
            {"group", "Metalloids"},
            {"protons", 32},
            {"electrons", 32},
            {"neutrons", 36},
            {"weight", 73}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "As"},
            {"name", "Arsenic"},
            {"group", "Metalloids"},
            {"protons", 33},
            {"electrons", 33},
            {"neutrons", 42},
            {"weight", 75}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Se"},
            {"name", "Selenium"},
            {"group", "Other Non-Metal"},
            {"protons", 34},
            {"electrons", 34},
            {"neutrons", 40},
            {"weight", 79}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Br"},
            {"name", "Bromine"},
            {"group", "Halogens"},
            {"protons", 35},
            {"electrons", 35},
            {"neutrons", 44},
            {"weight", 80}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Kr"},
            {"name", "Krypton"},
            {"group", "Noble Gases"},
            {"protons", 36},
            {"electrons", 36},
            {"neutrons", 42},
            {"weight", 84}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Rb"},
            {"name", "Rubidium"},
            {"group", "Alkali Metals"},
            {"protons", 37},
            {"electrons", 37},
            {"neutrons", 48},
            {"weight", 85}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sr"},
            {"name", "Strontium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 38},
            {"electrons", 38},
            {"neutrons", 46},
            {"weight", 88}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Y"},
            {"name", "Yttrium"},
            {"group", "Transition Metals"},
            {"protons", 39},
            {"electrons", 39},
            {"neutrons", 50},
            {"weight", 89}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Zr"},
            {"name", "Zirconium"},
            {"group", "Transition Metals"},
            {"protons", 40},
            {"electrons", 40},
            {"neutrons", 50},
            {"weight", 91}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Nb"},
            {"name", "Niobium"},
            {"group", "Transition Metals"},
            {"protons", 41},
            {"electrons", 41},
            {"neutrons", 52},
            {"weight", 93}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Mo"},
            {"name", "Molybdenum"},
            {"group", "Transition Metals"},
            {"protons", 42},
            {"electrons", 42},
            {"neutrons", 50},
            {"weight", 96}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Tc"},
            {"name", "Technetium"},
            {"group", "Transition Metals"},
            {"protons", 43},
            {"electrons", 43},
            {"neutrons", 54},
            {"weight", 98}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ru"},
            {"name", "Ruthenium"},
            {"group", "Transition Metals"},
            {"protons", 44},
            {"electrons", 44},
            {"neutrons", 52},
            {"weight", 101}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Rh"},
            {"name", "Rhodium"},
            {"group", "Transition Metals"},
            {"protons", 45},
            {"electrons", 45},
            {"neutrons", 58},
            {"weight", 103}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pd"},
            {"name", "Palladium"},
            {"group", "Transition Metals"},
            {"protons", 46},
            {"electrons", 46},
            {"neutrons", 56},
            {"weight", 106}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ag"},
            {"name", "Silver"},
            {"group", "Transition Metals"},
            {"protons", 47},
            {"electrons", 47},
            {"neutrons", 60},
            {"weight", 108}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cd"},
            {"name", "Cadmium"},
            {"group", "Transition Metals"},
            {"protons", 48},
            {"electrons", 48},
            {"neutrons", 58},
            {"weight", 112}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "In"},
            {"name", "Indium"},
            {"group", "Post Transition Metals"},
            {"protons", 49},
            {"electrons", 49},
            {"neutrons", 62},
            {"weight", 115}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sn"},
            {"name", "Tin"},
            {"group", "Post Transition Metals"},
            {"protons", 50},
            {"electrons", 50},
            {"neutrons", 62},
            {"weight", 119}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sb"},
            {"name", "Antimony"},
            {"group", "Metalloids"},
            {"protons", 51},
            {"electrons", 51},
            {"neutrons", 70},
            {"weight", 122}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Te"},
            {"name", "Tellurium"},
            {"group", "Metalloids"},
            {"protons", 52},
            {"electrons", 52},
            {"neutrons", 68},
            {"weight", 128}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "I"},
            {"name", "Iodine"},
            {"group", "Halogens"},
            {"protons", 53},
            {"electrons", 53},
            {"neutrons", 70},
            {"weight", 127}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Xe"},
            {"name", "Xenon"},
            {"group", "Noble Gases"},
            {"protons", 54},
            {"electrons", 54},
            {"neutrons", 70},
            {"weight", 131}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cs"},
            {"name", "Cesium"},
            {"group", "Alkali Metals"},
            {"protons", 55},
            {"electrons", 55},
            {"neutrons", 74},
            {"weight", 133}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ba"},
            {"name", "Barium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 56},
            {"electrons", 56},
            {"neutrons", 74},
            {"weight", 137}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "La"},
            {"name", "Lanthanum"},
            {"group", "Lanthonoids"},
            {"protons", 57},
            {"electrons", 57},
            {"neutrons", 81},
            {"weight", 139}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ce"},
            {"name", "Cerium"},
            {"group", "Lanthonoids"},
            {"protons", 58},
            {"electrons", 58},
            {"neutrons", 78},
            {"weight", 140}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pr"},
            {"name", "Praseodymium"},
            {"group", "Lanthonoids"},
            {"protons", 59},
            {"electrons", 59},
            {"neutrons", 82},
            {"weight", 141}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Nd"},
            {"name", "Neodymium"},
            {"group", "Lanthonoids"},
            {"protons", 60},
            {"electrons", 60},
            {"neutrons", 82},
            {"weight", 144}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pm"},
            {"name", "Promethium"},
            {"group", "Lanthonoids"},
            {"protons", 61},
            {"electrons", 61},
            {"neutrons", 83},
            {"weight", 145}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sm"},
            {"name", "Samarium"},
            {"group", "Lanthonoids"},
            {"protons", 62},
            {"electrons", 62},
            {"neutrons", 82},
            {"weight", 150}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Eu"},
            {"name", "Europium"},
            {"group", "Lanthonoids"},
            {"protons", 63},
            {"electrons", 63},
            {"neutrons", 88},
            {"weight", 152}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Gd"},
            {"name", "Gadolinium"},
            {"group", "Lanthonoids"},
            {"protons", 64},
            {"electrons", 64},
            {"neutrons", 88},
            {"weight", 157}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Tb"},
            {"name", "Terbium"},
            {"group", "Lanthonoids"},
            {"protons", 65},
            {"electrons", 65},
            {"neutrons", 94},
            {"weight", 159}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Dy"},
            {"name", "Dysprosium"},
            {"group", "Lanthonoids"},
            {"protons", 66},
            {"electrons", 66},
            {"neutrons", 90},
            {"weight", 163}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ho"},
            {"name", "Holmium"},
            {"group", "Lanthonoids"},
            {"protons", 67},
            {"electrons", 67},
            {"neutrons", 98},
            {"weight", 165}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Er"},
            {"name", "Erbium"},
            {"group", "Lanthonoids"},
            {"protons", 68},
            {"electrons", 68},
            {"neutrons", 94},
            {"weight", 167}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Tm"},
            {"name", "Thulium"},
            {"group", "Lanthonoids"},
            {"protons", 69},
            {"electrons", 69},
            {"neutrons", 100},
            {"weight", 169}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Yb"},
            {"name", "Ytterbium"},
            {"group", "Lanthonoids"},
            {"protons", 70},
            {"electrons", 70},
            {"neutrons", 98},
            {"weight", 173}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Lu"},
            {"name", "Lutetium"},
            {"group", "Lanthonoids"},
            {"protons", 71},
            {"electrons", 71},
            {"neutrons", 104},
            {"weight", 175}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Hf"},
            {"name", "Hafnium"},
            {"group", "Transition Metals"},
            {"protons", 72},
            {"electrons", 72},
            {"neutrons", 102},
            {"weight", 178}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ta"},
            {"name", "Tantalum"},
            {"group", "Transition Metals"},
            {"protons", 73},
            {"electrons", 73},
            {"neutrons", 107},
            {"weight", 181}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "W"},
            {"name", "Tungsten"},
            {"group", "Transition Metals"},
            {"protons", 74},
            {"electrons", 74},
            {"neutrons", 106},
            {"weight", 184}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Re"},
            {"name", "Rhenium"},
            {"group", "Transition Metals"},
            {"protons", 75},
            {"electrons", 75},
            {"neutrons", 110},
            {"weight", 186}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Os"},
            {"name", "Osmium"},
            {"group", "Transition Metals"},
            {"protons", 76},
            {"electrons", 76},
            {"neutrons", 108},
            {"weight", 190}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ir"},
            {"name", "Iridium"},
            {"group", "Transition Metals"},
            {"protons", 77},
            {"electrons", 77},
            {"neutrons", 114},
            {"weight", 192}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pt"},
            {"name", "Platinum"},
            {"group", "Transition Metals"},
            {"protons", 78},
            {"electrons", 78},
            {"neutrons", 112},
            {"weight", 195}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Au"},
            {"name", "Gold"},
            {"group", "Transition Metals"},
            {"protons", 79},
            {"electrons", 79},
            {"neutrons", 118},
            {"weight", 197}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Hg"},
            {"name", "Mercury"},
            {"group", "Transition Metals"},
            {"protons", 80},
            {"electrons", 80},
            {"neutrons", 116},
            {"weight", 201}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Tl"},
            {"name", "Thallium"},
            {"group", "Post Transition Metals"},
            {"protons", 81},
            {"electrons", 81},
            {"neutrons", 120},
            {"weight", 204}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pb"},
            {"name", "Lead"},
            {"group", "Post Transition Metals"},
            {"protons", 82},
            {"electrons", 82},
            {"neutrons", 122},
            {"weight", 207}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Bi"},
            {"name", "Bismuth"},
            {"group", "Post Transition Metals"},
            {"protons", 83},
            {"electrons", 83},
            {"neutrons", 124},
            {"weight", 209}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Po"},
            {"name", "Polonium"},
            {"group", "Metalloids"},
            {"protons", 84},
            {"electrons", 84},
            {"neutrons", 125},
            {"weight", 209}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "At"},
            {"name", "Astatine"},
            {"group", "Halogens"},
            {"protons", 85},
            {"electrons", 85},
            {"neutrons", 125},
            {"weight", 210}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Rn"},
            {"name", "Radon"},
            {"group", "Noble Gases"},
            {"protons", 86},
            {"electrons", 86},
            {"neutrons", 125},
            {"weight", 222}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Fr"},
            {"name", "Francium"},
            {"group", "Alkali Metals"},
            {"protons", 87},
            {"electrons", 87},
            {"neutrons", 136},
            {"weight", 223}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ra"},
            {"name", "Radium"},
            {"group", "Alkalin Earth Metals"},
            {"protons", 88},
            {"electrons", 88},
            {"neutrons", 135},
            {"weight", 226}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ac"},
            {"name", "Actinium"},
            {"group", "Actinoids"},
            {"protons", 89},
            {"electrons", 89},
            {"neutrons", 138},
            {"weight", 227}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Th"},
            {"name", "Thorium"},
            {"group", "Actinoids"},
            {"protons", 90},
            {"electrons", 90},
            {"neutrons", 138},
            {"weight", 232}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pa"},
            {"name", "Protactinium"},
            {"group", "Actinoids"},
            {"protons", 91},
            {"electrons", 91},
            {"neutrons", 140},
            {"weight", 231}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "U"},
            {"name", "Uranium"},
            {"group", "Actinoids"},
            {"protons", 92},
            {"electrons", 92},
            {"neutrons", 141},
            {"weight", 238}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Np"},
            {"name", "Neptunium"},
            {"group", "Actinoids"},
            {"protons", 93},
            {"electrons", 93},
            {"neutrons", 144},
            {"weight", 237}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Pu"},
            {"name", "Plutonium"},
            {"group", "Actinoids"},
            {"protons", 94},
            {"electrons", 94},
            {"neutrons", 144},
            {"weight", 244}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Am"},
            {"name", "Americium"},
            {"group", "Actinoids"},
            {"protons", 95},
            {"electrons", 95},
            {"neutrons", 146},
            {"weight", 243}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cm"},
            {"name", "Curium"},
            {"group", "Actinoids"},
            {"protons", 96},
            {"electrons", 96},
            {"neutrons", 147},
            {"weight", 247}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Bk"},
            {"name", "Berkelium"},
            {"group", "Actinoids"},
            {"protons", 97},
            {"electrons", 97},
            {"neutrons", 150},
            {"weight", 247}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cf"},
            {"name", "Californium"},
            {"group", "Actinoids"},
            {"protons", 98},
            {"electrons", 98},
            {"neutrons", 151},
            {"weight", 251}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Es"},
            {"name", "Einsteinium"},
            {"group", "Actinoids"},
            {"protons", 99},
            {"electrons", 99},
            {"neutrons", 153},
            {"weight", 252}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Fm"},
            {"name", "Fermium"},
            {"group", "Actinoids"},
            {"protons", 100},
            {"electrons", 100},
            {"neutrons", 157},
            {"weight", 257}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Md"},
            {"name", "Mendelevium"},
            {"group", "Actinoids"},
            {"protons", 101},
            {"electrons", 101},
            {"neutrons", 155},
            {"weight", 258}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "No"},
            {"name", "Nobelium"},
            {"group", "Actinoids"},
            {"protons", 102},
            {"electrons", 102},
            {"neutrons", 157},
            {"weight", 259}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Lr"},
            {"name", "Lawrencium"},
            {"group", "Actinoids"},
            {"protons", 103},
            {"electrons", 103},
            {"neutrons", 159},
            {"weight", 262}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Rf"},
            {"name", "Rutherfordium"},
            {"group", "Transition Metals"},
            {"protons", 104},
            {"electrons", 104},
            {"neutrons", 163},
            {"weight", 267}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Db"},
            {"name", "Dubnium"},
            {"group", "Transition Metals"},
            {"protons", 105},
            {"electrons", 105},
            {"neutrons", 163},
            {"weight", 268}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Sg"},
            {"name", "Seaborgium"},
            {"group", "Transition Metals"},
            {"protons", 106},
            {"electrons", 106},
            {"neutrons", 163},
            {"weight", 269}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Bh"},
            {"name", "Bohrium"},
            {"group", "Transition Metals"},
            {"protons", 107},
            {"electrons", 107},
            {"neutrons", 162},
            {"weight", 269}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Hs"},
            {"name", "Hassium"},
            {"group", "Transition Metals"},
            {"protons", 108},
            {"electrons", 108},
            {"neutrons", 161},
            {"weight", 269}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Mt"},
            {"name", "Meitnerium"},
            {"group", "Transition Metals"},
            {"protons", 109},
            {"electrons", 109},
            {"neutrons", 169},
            {"weight", 278}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ds"},
            {"name", "Darmstadtium"},
            {"group", "Transition Metals"},
            {"protons", 110},
            {"electrons", 110},
            {"neutrons", 171},
            {"weight", 281}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Rg"},
            {"name", "Roentgenium"},
            {"group", "Transition Metals"},
            {"protons", 111},
            {"electrons", 111},
            {"neutrons", 171},
            {"weight", 282}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Cn"},
            {"name", "Copernicium"},
            {"group", "Transition Metals"},
            {"protons", 112},
            {"electrons", 112},
            {"neutrons", 173},
            {"weight", 285}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Nh"},
            {"name", "Nihonium"},
            {"group", "Post Transition Metals"},
            {"protons", 113},
            {"electrons", 113},
            {"neutrons", 173},
            {"weight", 286}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Fl"},
            {"name", "Flerovium"},
            {"group", "Post Transition Metals"},
            {"protons", 114},
            {"electrons", 114},
            {"neutrons", 175},
            {"weight", 289}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Mc"},
            {"name", "Moscovium"},
            {"group", "Post Transition Metals"},
            {"protons", 115},
            {"electrons", 115},
            {"neutrons", 175},
            {"weight", 290}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Lv"},
            {"name", "Livermorium"},
            {"group", "Post Transition Metals"},
            {"protons", 116},
            {"electrons", 116},
            {"neutrons", 177},
            {"weight", 293}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Ts"},
            {"name", "Tennessine"},
            {"group", "Metalloids"},
            {"protons", 117},
            {"electrons", 117},
            {"neutrons", 177},
            {"weight", 294}
        },
        new Dictionary<string, object>()
        {
            {"symbol", "Og"},
            {"name", "Oganesson"},
            {"group", "Noble Gases"},
            {"protons", 118},
            {"electrons", 118},
            {"neutrons", 176},
            {"weight", 294}
        }
    };
}

public class DataStore : MonoBehaviour
{
    private void Awake()
    {
        Database.Load();
    }
}