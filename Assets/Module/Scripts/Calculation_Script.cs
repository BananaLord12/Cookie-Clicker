using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using rnd = UnityEngine.Random;
using UnityEngine;
using KModkit;

public class Calculation_Script {

    Cookie_Clicker Cookie;

    public Calculation_Script(Cookie_Clicker Cookie)
    {
        this.Cookie = Cookie;
    }

    public string[][] Achievements = new string[14][]
    {
            new string[]{ "Wake and bake","Making some dough","So baked right now","Fledgling bakery","Affluent bakery","World-famous bakery","Casmic bakery","Galactic bakery","Universal bakery","Timeless bakery","Infinite bakery"},
            new string[]{ "Grandma's cookies","Sloppy kisses","Retirement home","Friend of the ancients","Ruler of the ancients","The old never bothered me anyway","The agemaster","To odly go","Aged well","101st birthday","Defense of the ancients"},
            new string[]{ "My first farm","Reap what you sow","Farm ill","Perfected agriculture","Homegrown","Gardener extraordinaire","Seedy business","You and the beanstalk","Harvest moon","Make like a tree","Sharpest tool in the shed"},
            new string[]{ "You know the drill","Excavation site","Hollow the planet","Can you dig it","Center of the Earth","Tectonic ambassador","Freak fracking","Tomancind the stone","Mine?","Cave story","Hey now, you're a rock"},
            new string[]{ "Production chain","Industrial revolution","Global warming","Ultimate automation","Technocracy","Rise of the machines","Modern times","Ex machina","in full gear","in-cog-neato","Beak the mold"},
            new string[]{ "Pretty penny","Fit the bill","A loat in the dark","Need for greed","It's the economy, stupid","Acquire currency","The never of war","And i need it now","Treacle tart economics","Save your breath because\n that's all you've got left","Get the show on, get paid"},
            new string[]{ "Your time to shrine","Shady sect","New-age cult","Organized religion","Fanaticism","Zealotry","Wololo","Pray on the weak","Holy cookies, grandma!","Vengeful and almighty","My world's on fire, how about yours"},
            new string[]{ "Bewitched","The sorcerer's apprentice","Charms and enchantments","Curses and maledictions","Magic kingdom","The wizarding world","And now for my next trick, I'll\n need a volunteer from the audience","It's a kind of magic","The Prestige","Spell it out for you","The meteor men beg to differ"},
            new string[]{ "Expedition","Galactic highway","Far far away","Type II civilization","We come in peave","Parsec-masher","It's not delivery","Make it so","That's just peanuts to space","Space space space space space","Only shooting stars"},
            new string[]{ "Transmutation","Transmogrification","Gold member","Gild wars","The secrets of the universe","The work of a lifetime","Gold, Jerry! Gold","Worth its weight in lead","Don't get used to yourself,\n you're gonna have to change","We could all use a little change"},
            new string[]{ "A whole new world","Now you're thinking","Dimensional shift","Brain-split","Realm of the Mad God","A place lost in time","A place lost in time","Forbidden zone","H̸̷͓̳̳̯̟͕̟͍͍̣͡ḛ̢̦̰̺̮̝͖͖̘̪͉͘͡ ̠̦͕̤̪̝̥̰̠̫̖̣͙̬͘ͅC̨̦̺̩̲̥͉̭͚̜̻̝̣̼͙̮̯̪o̴̡͇̘͎̞̲͇̦̲͞͡m̸̩̺̝̣̹̱͚̬̥̫̳̼̞̘̯͘ͅẹ͇̺̜́̕͢s̶̙̟̱̥̮̯̰̦͓͇͖͖̝͘͘͞","What happens in the vortex stays in the vortex","Objects in the mirror dimension\n are closer than they appear","Your brain gets smart but your head gets dumb"},
            new string[]{ "Time warp","Alternate timeline","Rewriting history","Time duke","Forever and ever","Heat death", "cookie clicker forever and forever\n a hundred years cookie clicker,\n over and over cookie\n clicker adventures dot com", "Way back then","Invited to yesterday's party","Groundhog day","The years start coming"},
            new string[]{ "Antibatter","Quirky quarks","it does matter!","Molecular maestro","Walk the planck","Microcosm","Scientists baffled everywhere","Exotic matter","Downsizing","A matter of perspective","What a concept"},
            new string[]{ "Lone photon","Dazzling glimmer","Blinding flash","Unending glow","Rise and shine","Bright future","Harmony of the spheres","At the end of the tunnel","My eyes","Optical illusion","You'll never shine if you don't glow"}
    };

    public double CalculateBasePrice(int Multiplier, long BaseCost){
        return Math.Ceiling(BaseCost * (Math.Pow(1.15f, Multiplier)-1)/0.15);
    }

    public string ConvertToScientific(double Num){
        return Num.ToString("0.000E0");
    }

    public void SetAchievement(int Building, int Achievement){
        string achievement = Achievements[Building][Achievement];
        Sprite[] Achie_Sprites = GetSprites(Building);
        Cookie.Achi_template.GetComponent<Renderer>().enabled = true;
        Cookie.Achi_text.text = achievement;
        Cookie.Achi_text.GetComponent<Renderer>().enabled = true;
        Cookie.Achi_Sprite.enabled = true;
        Cookie.Achi_Sprite.sprite = Achie_Sprites[Achievement];
        return;
    }

    public Sprite[] GetSprites(int Building)
    {
        switch (Building)
        {
            case 0:
                return Cookie.Totalcookies;
            case 1:
                return Cookie.Grandmas;
            case 2:
                return Cookie.Farms;
            case 3:
                return Cookie.Mines;
            case 4:
                return Cookie.Factories;
            case 5:
                return Cookie.Banks;
            case 6:
                return Cookie.Temples;
            case 7:
                return Cookie.Wizard_Towers;
            case 8:
                return Cookie.Shipments;
            case 9:
                return Cookie.Alchemy_Labs;
            case 10:
                return Cookie.Portals;
            case 11:
                return Cookie.Time_Machines;
            case 12:
                return Cookie.Antimater_Condensors;
            case 13:
                return Cookie.Prisms;
            default:
                return null;

        }
    }

    public string GetAchievement(int Building, int Achievement){
        return Achievements[Building][Achievement];
    }
}
