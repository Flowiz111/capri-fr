using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CapriDesktop
{
    internal static class SelfKnow
    {
        internal sealed class Topic
        {
            internal readonly string Name;
            internal readonly string PlainName;
            internal readonly string[] Words;
            internal readonly string Facts;
            internal readonly string File;
            internal readonly string Label;
            // Facts made from her tools here, for topics about what she can do.
            private readonly Func<Hands.Kit, string> made;

            internal Topic(string name, string words, string facts, string file, string label)
            {
                Name = name;
                PlainName = Plain(name);
                Words = words.Split('|');
                Facts = facts;
                File = file;
                Label = label;
            }

            internal Topic(string name, string words, Func<Hands.Kit, string> made, string file, string label) : this(name, words, (string)null, file, label) { this.made = made; }

            internal string FactsFor(Hands.Kit kit) { return made == null ? Facts : made(kit); }
        }

        internal static readonly Topic[] Topics =
        {
            new Topic("What's new", "whats new in this update|whats new in the update|what is new in this update|what is new in the update|what did the update add|what did this update add|what changed in this update|what changed in the update|whats in this update|whats in the update|whats new in this version|update notes|patch notes|release notes",
                News.Facts, null, null),
            new Topic("Mainframe", "your settings|her settings|open your settings|mainframe|settings window|tes reglages|ouvre tes reglages|ses reglages|fenetre des reglages",
                "Fais un clic droit sur Capri ou sur son icône et choisis Ouvrir Capri FR Mainframe. La page principale contient CERVEAU, VOIX et CAPRI ; Réglages, en haut, ouvre la page des réglages. Les changements sont enregistrés immédiatement.", "Capri.cs", "Open Capri FR Mainframe"),
            new Topic("Settings pages", "settings button|back button in mainframe|settings pages|bouton reglages|pages de reglages",
                "Réglages, en haut du Mainframe, ouvre sa page de réglages. Retour ramène à CERVEAU, VOIX et CAPRI. Réglages avancés montre toutes les lignes ; Réglages simples garde l'essentiel. Les changements sont enregistrés immédiatement.", "Mainframe.cs", "Settings"),
            new Topic("Mods", "mods page|mods button|mod menu|your mods|capri mods|page des mods|bouton mods|tes mods",
                "Mods, dans la section CAPRI du Mainframe, ouvre sa page de mods. Un fichier .capri-mod change seulement des réglages. Un fichier .capri-code exécute du code avec un accès complet au PC : elle demande donc avant de le lancer, et de nouveau s'il est modifié. Retour ramène à CERVEAU, VOIX et CAPRI.", "Mainframe.cs", "Mods"),
            new Topic("Import mod", "import a mod|import mod|install a mod|add a mod|importer un mod|installer un mod|ajouter un mod",
                "Importer un mod permet de choisir un fichier .capri-mod, .capri-code ou .capri-skin et de l'activer. Les mods de code demandent d'abord, et une apparence change son look. Un mod de même identifiant est remplacé, et un mod abîmé est refusé avec la raison.", "MainframeMods.cs", "Import mod"),
            new Topic("Browse mods", "browse mods|mods directory|find mods|capri-mods|parcourir les mods|annuaire des mods|trouver des mods",
                "Parcourir les mods ouvre la fenêtre des mods : le contenu de l'annuaire sur GitHub, avec images, noms, auteurs, descriptions, téléchargements et notes. La recherche filtre pendant la saisie ; la connexion à Internet n'a lieu que si tu appuies sur Parcourir les mods.", "MainframeMods.cs", "Browse mods"),
            new Topic("Capri VR", "capri vr|capri room|virtual reality|meta quest|quest 3|in vr|vr version|oculus|realite virtuelle|en vr|version vr",
                "Capri Room est sa version en réalité mixte pour Meta Quest 3 et 3S, créée par fadievr : elle marche dans ta vraie pièce et s'assoit sur tes meubles. C'est le fichier Capri-VR-Quest.apk de la page GitHub du projet d'origine. Installe-le en mode développeur avec SideQuest ou Meta Quest Developer Hub, puis ouvre-le depuis Bibliothèque > Sources inconnues et autorise les données spatiales et le micro. Dans le casque, Y ouvre ses réglages.", null, null),
            new Topic("Search mods", "search mods|search the directory|chercher des mods|chercher dans l annuaire",
                "La recherche filtre l'annuaire des mods par nom, auteur, description, dossier ou fichier pendant la saisie. Un mod doit correspondre à tous les mots.", "MainframeModsWindow.cs", "Search mods"),
            new Topic("Mod filter", "filter mods|only skins|only code mods|filtrer les mods",
                "Tous, Réglages, Code et Apparences n'affichent que ce type de mod, et fonctionnent avec la recherche.", "MainframeModsWindow.cs", "Mod filter"),
            new Topic("Install mod", "install from the directory|download a mod|telecharger un mod",
                "Installer télécharge le mod, vérifie qu'il correspond à l'annuaire et l'importe. Les mods de code demandent toujours avant de s'exécuter.", "MainframeModsWindow.cs", "Install mod"),
            new Topic("View mod", "view a mod|read a mod|see a mod on github|voir un mod|lire un mod",
                "Voir ouvre le dossier du mod sur GitHub dans ton navigateur, pour lire sa description et son code avant de l'installer.", "MainframeModsWindow.cs", "View mod"),
            new Topic("Rate mod", "rate a mod|mod rating|mod stars|noter un mod|note du mod",
                "Une fois un mod de la fenêtre installé, clique sur ses étoiles pour le noter de 1 à 5. Une nouvelle note remplace l'ancienne.", "MainframeModsWindow.cs", "Rate mod"),
            new Topic("Publish mod", "publish a mod|share my mod|share a mod|upload a mod|publier un mod|partager mon mod|partager un mod",
                "Publier, sur un mod importé, explique comment le partager : forke capri-mods sur GitHub, ajoute un dossier avec le mod, un README et un preview.png, puis ouvre une pull request.", "MainframeMods.cs", "Publish mod"),
            new Topic("Open mods folder", "mods folder|where are mods|dossier des mods|ou sont les mods",
                "Dossier des mods montre où sont gardés ses mods importés, à côté de ses réglages.", "MainframeMods.cs", "Open mods folder"),
            new Topic("Mod switch", "turn off a mod|turn on a mod|disable a mod|enable a mod|desactiver un mod|activer un mod",
                "Oui et Non activent ou coupent un mod sans le supprimer. Le changement est immédiat et reste après un redémarrage.", "MainframeMods.cs", "Mod switch"),
            new Topic("Her colors", "change your color|change your colour|change your hair color|recolor|recolour|her colors|change ta couleur|changer ta couleur|ses couleurs|tes couleurs",
                "Sur la page Mods, choisis une partie (cheveux, accents, yeux, contours ou corps), puis une couleur. Cela s'applique tout de suite à toutes ses animations et apparences, et s'enregistre dans un mod Mes couleurs que tu peux partager.", "MainframeMods.cs", "Her colors"),
            new Topic("Reset colors", "reset your colors|reset your colours|original colors|retablir les couleurs|couleurs d origine",
                "Rétablir les couleurs lui redonne ses couleurs d'origine : les couleurs choisies sont supprimées et les mods de couleur désactivés.", "MainframeMods.cs", "Reset colors"),
            new Topic("Remove mod", "remove a mod|delete a mod|uninstall a mod|supprimer un mod|desinstaller un mod",
                "Supprimer retire le mod de son dossier et annule ses réglages. Un mod de code est arrêté, mais ce qu'il a déjà fait reste fait. Le fichier importé reste à sa place.", "MainframeMods.cs", "Remove"),
            new Topic("Mod pages", "more mods|next page of mods|plus de mods|page suivante de mods",
                "Retour et Suivant parcourent la liste quand il y a plus de mods que la page ne peut en afficher.", "MainframeMods.cs", "Mod pages"),
            new Topic("Minimize Mainframe", "minimize mainframe|minimise mainframe|minimize your settings|reduire le mainframe",
                "Réduire place Capri FR Mainframe dans la barre des tâches. Capri continue de tourner, et le bouton de la barre des tâches rouvre la fenêtre.", "Mainframe.cs", "Minimize"),
            new Topic("Close Mainframe", "close mainframe|close your settings|fermer le mainframe",
                "Fermer place aussi Capri FR Mainframe dans la barre des tâches, et Capri continue de tourner. Utilise Quitter Capri sur la page principale pour la fermer complètement.", "Mainframe.cs", "Close"),
            new Topic("Help", "your help button|capri help page|help website|bouton aide|page d aide",
                "Mainframe > CAPRI > Aide ouvre la page d'aide de Capri FR, en français, dans ton navigateur. Tu peux aussi poser une question sur un réglage directement à Capri.", "Mainframe.cs", "Help"),
            new Topic("Quit Capri", "quit capri|quit button|exit capri|quitter capri|bouton quitter",
                "Mainframe > CAPRI > Quitter Capri ferme Capri et ses fenêtres. Elle s'arrête jusqu'à ce que tu la rouvres. Le bouton du coin du Mainframe, lui, ne fait que réduire la fenêtre.", "Mainframe.cs", "Quit Capri"),
            new Topic("What she can do", "what can you do|what do you do|what can capri do|your features|your capabilities|what are your tools|what tools do you have|your tools|everything you can do|what else can you do",
                delegate(Hands.Kit kit) { return "Talk by hovering over Capri and pressing TYPE or TALK, or use the keys in Mainframe > Réglages > TOUCHES. She chats and speaks, and with her tools she can " + kit.Abilities + ". Ask in plain words; Esc stops a running job. With a cloud brain she also changes her own settings when you ask, like make yourself bigger or stop wandering, and one app's own volume, like turn the game down." + (kit.Absent.Length == 0 ? "" : " Not here right now: " + Regex.Replace(kit.Absent, @"\btheir\b", "your") + ".") + (kit.Switched.Length == 0 ? "" : " Off right now: " + Regex.Replace(kit.Switched, @"\btheir\b", "your") + "."); }, "Hands.cs", "use_computer"),
            new Topic("Clones and chaos mode", "your clones|clone yourself|chaos mode|make clones|copies of yourself|copy yourself|multiply yourself|duplicate yourself|tes clones|clone toi|mode chaos|fais des clones|copies de toi",
                "Demande « clone-toi », « fais 5 clones » ou « active le mode chaos ». Ses clones se baladent, se parlent, volent le curseur et attrapent des fenêtres ; sans nombre, ils se multiplient jusqu'à ce que tu les effaces. Maintiens Échap, ou dis « débarrasse-toi de tes clones » ou « mode chaos non » : les fenêtres déplacées reviennent. Mainframe > Réglages > CAPRI > Chaos si elle est vexée peut les lâcher quand tu l'insultes. Windows uniquement pour l'instant.", "Hands.cs", "clone"),
            new Topic("Shortcuts she can press", "shortcuts can you press|shortcuts you can press|shortcuts do you press|shortcuts do you know|press shortcuts|press a shortcut|press keyboard shortcuts",
                delegate(Hands.Kit kit) { return "Ask in plain words and she presses it for you: she can " + Pressed(kit) + ". Mainframe > Réglages > HABITUDES > Security > Shortcuts sets whether she asks first. Her own hotkeys, like Talk and Quiet, are under Mainframe > Réglages > TOUCHES."; }, "Hands.cs", "shortcut"),
            new Topic("Arrange windows", "move my window|move a window|move windows|snap my window|snap a window|snap windows|maximize a window|maximise a window|maximize my window|resize my window|resize a window|window to my other monitor|window to another monitor|windows between monitors|arrange my windows|arrange windows|deplace ma fenetre|deplacer une fenetre|ranger mes fenetres|maximise la fenetre|redimensionne la fenetre",
                "Demande-lui d'agrandir, restaurer, ancrer à gauche, à droite, en haut ou en bas, envoyer sur un autre écran, placer dans un coin ou au centre, redimensionner une fenêtre, ou la garder au premier plan. Par exemple : « ancre Discord à gauche », « agrandis Chrome ». Les écrans sont nommés de gauche à droite : écran principal, écran 2, écran 3. Les fenêtres lancées en administrateur ne bougent que si Capri l'est aussi. Mainframe > Réglages > HABITUDES > Sécurité > Fermer et changer d'applis et d'onglets décide si elle demande d'abord. Windows uniquement pour l'instant.", "Hands.cs", "app_window"),
            new Topic("Look at another monitor", "look at my other monitor|look at another monitor|see my other monitor|see my second monitor|look at my second monitor|see all my monitors|look at all my monitors|which monitor do you look at|which screen do you look at|regarde mon autre ecran|regarde mon deuxieme ecran|quel ecran regardes tu",
                "Avec plusieurs écrans, elle regarde par défaut celui de la fenêtre que tu utilises. Nomme-en un autre et elle y regarde : « qu'y a-t-il sur mon deuxième écran ». Les écrans s'appellent écran principal, écran 2 et écran 3, de gauche à droite. Il lui faut un cerveau qui voit. Windows uniquement.", "Hands.cs", "look_at_screen"),
            new Topic("Settings she can change", "change your own settings|change your settings yourself|settings can you change|settings you can change|can you change your settings|make yourself bigger|make yourself smaller|make yourself louder|make yourself quieter|change tes reglages|quels reglages peux tu changer|fais toi plus grande|fais toi plus petite",
                "Avec un cerveau cloud, elle change ses propres réglages quand tu le demandes et dit où ils en sont : sa voix, le silence, Prendre de mes nouvelles, la danse, les caresses, le chaos, le mini-lecteur, le son de frappe, les messages de relance, les clips, les réactions, le mode jeu, l'hologramme, Regarder mon écran, les commentaires, ainsi que la taille, les balades, les apparitions, le sommeil, le volume, la vitesse, les réponses et la bulle. Essaie « fais-toi plus grande » ou « arrête de te balader ». Les clés, les cerveaux, le service vocal, la sécurité et Retenir nos discussions se changent uniquement dans le Mainframe.", "Hands.cs", "capri_settings"),
            new Topic("Brain", "your brain|her brain|brain setup|set up her brain|normal brain|higher brain|max intelligence|ollama|local brain|built in brain|ton cerveau|son cerveau|installer son cerveau|cerveau local|ollama|intelligence max",
                "Dans Mainframe > CERVEAU, NORMAL utilise un cerveau local : appuie sur Installer son cerveau pour en télécharger un, ou ouvre Ollama et appuie sur Actualiser. INTELLIGENCE MOYENNE est gratuite avec une clé NVIDIA, Google AI Studio (Gemini) ou OpenRouter. INTELLIGENCE MAX accepte aussi OpenAI, Claude ou DeepSeek. Seul le niveau choisi tourne. Choisis un modèle dans le menu ; une clé de cerveau et une clé de voix ont des rôles différents.", "Mainframe.cs", "NORMAL"),
            new Topic("Medium intelligence", "medium intelligence|medium brain|higher brain|higher tier|the higher option|what happened to higher|free brain|free api|intelligence moyenne|cerveau gratuit|api gratuite",
                "Mainframe > CERVEAU > INTELLIGENCE MOYENNE est gratuite avec une clé NVIDIA, Google AI Studio (Gemini) ou OpenRouter. Ces clés peuvent être lentes et limitées. L'API gratuite de NVIDIA est souvent lente aux heures de pointe, et la plupart de ses modèles ne voient pas ton écran. Gemini démarre sur un modèle Flash disponible. OpenRouter n'affiche que les modèles en :free et préfère ceux qui voient et utilisent des outils.", "Mainframe.cs", "INTELLIGENCE MOYENNE"),
            new Topic("Brain models", "brain model picker|brain model menu|choose a brain model|modele du cerveau|choisir un modele",
                "Le menu de modèles de Mainframe > CERVEAU choisit le cerveau utilisé. Les choix locaux viennent des modèles installés et des serveurs locaux détectés, les choix cloud du service et de la clé choisis. Seul le cerveau choisi tourne.", "Mainframe.cs", "Brain model"),
            new Topic("Refresh", "refresh your brain|refresh brain models|refresh button|bouton actualiser|actualiser le cerveau",
                "Mainframe > CERVEAU > Actualiser cherche les modèles Ollama installés, son modèle intégré et les serveurs locaux connus, puis met à jour le menu. Avec un cerveau cloud, cela actualise aussi les modèles de ce service.", "Mainframe.cs", "Refresh"),
            new Topic("Installer son cerveau", "brain download button|set up her brain button|cancel brain download|telecharger le cerveau|annuler le telechargement du cerveau",
                "Mainframe > CERVEAU > Installer son cerveau télécharge un modèle local. Si Ollama tourne, elle passe par lui ; sinon, elle installe son cerveau intégré, le charge et enregistre le choix. La progression s'affiche, et un nouvel appui annule.", "Mainframe.cs", "Installer son cerveau"),
            new Topic("Clé gratuite", "free key|nvidia key|nvidia api|connect a key|connect my key|connect an api key|brain key|api keys|cle gratuite|cle nvidia|connecter une cle|cle api|cle de cerveau",
                "Dans Mainframe > CERVEAU, Clé gratuite propose NVIDIA, Google AI Studio (Gemini) et OpenRouter. Choisis un fournisseur pour ouvrir sa page, crée une clé et copie-la. Choisis INTELLIGENCE MOYENNE, colle la clé dans la case de CERVEAU, appuie sur Connecter, puis choisis un modèle. Les clés se collent dans le Mainframe, pas dans la discussion, et restent chiffrées sur ce PC. Avec une clé enregistrée et une case vide, Oublier la retire.", "Mainframe.cs", "Clé gratuite"),
            new Topic("Brain API key", "brain key box|forget brain key|brain api key box|case de la cle|oublier la cle du cerveau",
                "Mainframe > CERVEAU > Connecter enregistre la clé API collée, chiffrée sur ce PC. INTELLIGENCE MOYENNE accepte NVIDIA, Google AI Studio (Gemini) et OpenRouter ; INTELLIGENCE MAX accepte toutes les clés cloud prises en charge. Avec une clé enregistrée et une case vide, Oublier la retire.", "Mainframe.cs", "NVIDIA, Google AI Studio (Gemini) or OpenRouter API key"),
            new Topic("Provider", "provider button|brain provider|my own provider|openai compatible provider|openai-compatible provider|custom provider|lm studio address|bouton fournisseur|fournisseur de cerveau|mon propre fournisseur|fournisseur compatible openai",
                "Mainframe > CERVEAU > Fournisseur ouvre la fenêtre pour tout serveur ou service compatible OpenAI, comme LM Studio ou OpenRouter. Saisis son adresse et sa clé si besoin, appuie sur Connecter, puis choisis son modèle dans le menu de CERVEAU. L'adresse et la clé restent chiffrées sur ce PC, et la clé ne part que vers cette adresse.", "Mainframe.cs", "Provider"),
            new Topic("Move a paid brain key", "move paid key|move to max|paid key in medium|deplacer la cle payante|cle payante",
                "Une clé OpenAI, Anthropic ou DeepSeek collée sous INTELLIGENCE MOYENNE affiche : « C'est une clé payante : mets-la sous INTELLIGENCE MAX ». Appuie sur Passer en MAX pour la connecter en un clic. Ta clé actuelle reste active jusqu'à ce que la nouvelle soit connectée.", "Mainframe.cs", "Move key to INTELLIGENCE MAX"),
            new Topic("Cloud brain keys", "openai key|claude key|deepseek key|chatgpt key|paid key|paid brain|google ai|gemini|openrouter|open router|cle openai|cle claude|cle deepseek|cle payante|cerveau payant|gemini|openrouter",
                "Dans Mainframe > CERVEAU, choisis INTELLIGENCE MAX, colle une clé API Google AI (Gemini), OpenRouter, NVIDIA, OpenAI, Claude ou DeepSeek et appuie sur Connecter. MAX garde toute la liste de modèles, payants compris. Une nouvelle clé Claude démarre sur Claude Haiku 5.5. Ces services sont des cerveaux, distincts de VOIX CLOUD, et un abonnement ChatGPT n'est pas une clé API.", "Mainframe.cs", "INTELLIGENCE MAX"),
            new Topic("Voice", "change your voice|change her voice|your voice|her voice|voice choices|voice settings|prebuilt local|custom local|cloud voice|elevenlabs|fish audio|change ta voix|ta voix|sa voix|choix de voix|reglages de voix|elevenlabs|voix cloud|voix francaise",
                "Mainframe > VOIX propose NON pour le texte seul, LOCALE INTÉGRÉE pour les voix gratuites sur ce PC, LOCALE PERSO pour ton propre modèle, et VOIX CLOUD pour Fish Audio, ElevenLabs ou OpenAI. En français, elle parle avec une voix de Windows, féminine en priorité. D'autres voix s'ajoutent dans Paramètres Windows > Heure et langue > Voix. Tester, à côté du micro, teste ton entrée micro.", "Mainframe.cs", "LOCALE INTÉGRÉE"),
            new Topic("Voice off", "voice off setting|silent voice setting|voix coupee|reglage voix non",
                "Mainframe > VOIX > NON ne garde que le texte. Les clés et modèles enregistrés restent là. Choisis une autre ligne pour qu'elle reparle.", "Mainframe.cs", "OFF"),
            new Topic("Mode appel", "call mode|call panel|call pill|call captions|call app|zoom call|teams call|mode appel|panneau d appel|sous titres d appel|appli d appel|appel zoom|appel teams",
                "Mainframe > Réglages > VOIX > Appli d'appel choisit ce qu'elle entend. Auto préfère Discord, puis l'appli d'appel qui fait du son ; Navigateur et Steam se choisissent à la main. Tes demandes adressées à Capri utilisent ses outils en mode appel (bêta) : elle peut rejoindre un appel, transmettre un message et lancer le jeu choisi. Par défaut, tes amis peuvent seulement parler. Sécurité > Tes amis en appel demandent, tu confirmes est coupé par défaut ; activé, chaque demande attend 60 secondes ta confirmation.", "Mainframe.cs", "Mode appel (beta)"),
            new Topic("Call app", "call app|zoom call|teams call|appli d appel",
                "Mainframe > Réglages > VOIX > Appli d'appel choisit ce qu'elle entend. Auto préfère Discord, puis une appli d'appel qui fait du son. Navigateur et le chat vocal Steam se choisissent à la main, et Navigateur capte tous les onglets.", "Mainframe.cs", "Call app"),
            new Topic("Custom local voice", "custom local voice setup|your voice model panel|voix locale perso|ton modele vocal",
                "Mainframe > VOIX > LOCALE PERSO utilise un modèle vocal sur ton PC. Configurer ouvre son panneau, où tu choisis un serveur local ou des fichiers de modèle. Teste-le là, puis enregistre pour le sélectionner.", "Mainframe.cs", "LOCALE PERSO"),
            new Topic("Cloud voice service", "cloud voice service picker|choose a cloud voice service|service de voix cloud",
                "Mainframe > VOIX > VOIX CLOUD utilise Fish Audio, ElevenLabs ou OpenAI. Choisis le service dans son menu. Fish Audio et OpenAI gardent clé, modèle et voix dans un panneau ; ElevenLabs a ses réglages sur la page principale. Les clés de cerveau sont séparées.", "Mainframe.cs", "Cloud voice service"),
            new Topic("ElevenLabs key", "elevenlabs voice key|forget elevenlabs key|connect elevenlabs key|cle elevenlabs|oublier la cle elevenlabs",
                "La case de clé ElevenLabs dans Mainframe > VOIX prend une clé API vocale. Connecter la vérifie, charge tes voix et l'enregistre, chiffrée, sur ce PC. Avec une clé enregistrée et une case vide, Oublier coupe la liaison et retire la clé. Ta clé de cerveau ne change pas.", "Mainframe.cs", "ElevenLabs API key"),
            new Topic("Voice ID", "elevenlabs voice id|voice id box|voice id menu|id de voix",
                "L'ID de voix ElevenLabs, dans Mainframe > VOIX, choisit qui parle. Connecter charge les voix de ta clé. Choisis-en une ou colle son ID ; quitter la case l'enregistre. Vider la case retire l'ID.", "Mainframe.cs", "ElevenLabs voice ID"),
            new Topic("Speech model", "elevenlabs speech model|elevenlabs model menu|default speech model|modele vocal elevenlabs",
                "Le menu de modèles ElevenLabs, dans Mainframe > VOIX, liste les modèles de ta clé. Choisis-en un ou Modèle par défaut. Ce choix ne concerne que la voix ElevenLabs, pas le cerveau.", "Mainframe.cs", "Default model"),
            new Topic("Free voice style", "free voice style picker|free voice menu|choose a free voice style|style de voix gratuite|menu des voix gratuites",
                "Le menu de voix gratuites de Mainframe > VOIX > LOCALE INTÉGRÉE choisit un style de voix anglaise sur ton PC. Ta voix (HD) utilise un extrait choisi et demande plus de mémoire. En français, elle utilise plutôt les voix de Windows.", "Mainframe.cs", "Free voice: Her own"),
            new Topic("Get free voice", "get free voice button|cancel free voice download|download free voice|obtenir la voix gratuite|telecharger la voix gratuite",
                "Mainframe > VOIX > Obtenir la voix gratuite télécharge et prépare son modèle vocal local, en anglais. Il n'est pas nécessaire pour parler français, car les voix de Windows s'en chargent. Le bouton devient Annuler pendant l'opération, et les parties téléchargées sont gardées.", "Mainframe.cs", "Get free voice"),
            new Topic("Voice setup guide", "voice setup guide|cloud voice setup guide|guide de la voix",
                "Mainframe > VOIX > Guide ouvre le guide des clés ElevenLabs dans ton navigateur. Avec Fish Audio ou OpenAI, il ouvre plutôt le panneau de ce service.", "Mainframe.cs", "Setup guide"),
            new Topic("Sa sortie audio", "her speaker|your speaker|speaker picker|your audio output|your output device|your voice in my headphones|sa sortie audio|ta sortie audio|haut parleur|sortie son",
                "Mainframe > VOIX > Sa sortie audio choisit où jouent sa voix et ses sons ; Par défaut suit Windows. Une sortie absente reste enregistrée avec la mention (missing), et la sortie par défaut prend le relais jusqu'à son retour.", "Mainframe.cs", "Sa sortie audio"),
            new Topic("Test mic", "test my mic|test the mic|test your mic|mic test|test microphone|test button|is my mic working|tester le micro|teste mon micro|test du micro|mon micro marche",
                "Mainframe > VOIX > Tester, à côté du choix du micro, écoute jusqu'à 10 secondes et montre le niveau. Arrêter le termine plus tôt. Il indique si le micro est coupé, muet, très faible, ou si le son lui parvient.", "Mainframe.cs", "Test"),
            new Topic("HD voice", "hd voice|your voice hd|voice clip|clone my voice|voix hd|extrait de voix|cloner ma voix",
                "Le menu de voix gratuites de Mainframe > VOIX > LOCALE INTÉGRÉE propose Ta voix (HD) et le choix d'un extrait. La HD conseille 6 Go de mémoire graphique et 16 Go de RAM. Avec moins, elle prévient mais te laisse essayer. Utilise seulement ta voix, ou une voix dont tu as les droits et le consentement.", "Mainframe.cs", "Your voice (HD)"),
            new Topic("Microphone", "your microphone|your mic|voice chat|talk button|mute mic|mute your mic|push to talk|ton micro|discussion vocale|bouton parler|couper le micro|appuyer pour parler",
                "Survole Capri et appuie sur l'onde, ou utilise Ctrl + Alt + V, pour lancer ou couper la discussion vocale ; choisis l'entrée dans Mainframe > VOIX > Micro. Le bouton micro ou Ctrl + Alt + N le coupe, et Réglages > TOUCHES peut lier Appuyer pour parler à une touche ou un bouton de souris.", "Mainframe.cs", "Mic: default"),
            new Topic("Mic problems", "why cant you hear me|cant hear me|cannot hear me|my mic isnt working|mic not working|microphone not working|mic isnt detected|no mic|silent mic|mic too quiet|windows is blocking the mic|tu ne m entends pas|tu m entends pas|mon micro ne marche pas|micro ne marche pas|pas de micro|micro trop faible|windows bloque le micro",
                "Regarde sous Mainframe > VOIX > Micro : la ligne d'écoute indique le moteur, le micro ou l'erreur. Aucun micro trouvé propose les paramètres de son. Windows bloque le micro propose la confidentialité du micro : active l'accès au microphone et l'accès pour les applications de bureau. Silence ou micro trop faible propose Tester le micro et Choisir un autre micro.", "Mainframe.cs", "Hearing status"),
            new Topic("Hearing", "how do i change how you hear me|change how you hear|hearing choice|hearing engine|her own ears|windows speech|online speech recognition|speech recognition is off|different mic|transcription not working|comment tu m entends|moteur d ecoute|son ecoute|voix de windows|reconnaissance vocale",
                "Mainframe > Réglages > VOIX > Écoute propose Auto, Son écoute ou Voix de Windows. Auto utilise son écoute quand elle est prête, sinon la voix de Windows. Son écoute entend le micro choisi hors ligne et gère l'anglais et 24 autres langues européennes, dont le français, sur Windows 64 bits. La voix de Windows part du micro par défaut. Si la reconnaissance en ligne est coupée, une option ouvre ses paramètres.", "Mainframe.cs", "Hearing engine"),
            new Topic("Listen for Capri", "listen for capri|listen for your name|only reply to your name|say your name|ecouter son nom|ecoute ton nom|dis ton nom",
                "Mainframe > Réglages > VOIX > Écouter son nom est coupé par défaut. Activé, son micro reste ouvert tant qu'elle tourne, sans touche, et seules les phrases contenant son nom lui parviennent. Ctrl + Alt + V ferme ou rouvre ses oreilles. Masquée ou en silence, elle attend. « Arrête » et Appuyer pour parler marchent sans son nom.", "Mainframe.cs", "Listen for Capri"),
            new Topic("Listening to PC sound", "listen tool|pc sound|listen to videos|hear videos|hear game dialogue|translate speech|ecoute le son du pc|ecoute la video|traduis la video",
                "Demande-lui d'écouter, de dire ce qui se dit dans une vidéo, un appel ou un jeu, ou de traduire. L'outil capte la sortie par défaut pendant 15 secondes, ou la durée que tu donnes, garde l'audio en mémoire seulement et l'efface après transcription. Elle n'enregistre pas en arrière-plan, ne rejoue pas de son passé et ne reconnaît pas les chansons.", "Hands.cs", "listen"),
            new Topic("Follow-up messages", "follow up messages|follow-up messages|second thought|answer then yourself|messages de relance|seconde pensee",
                "Mainframe > Réglages > CERVEAU > Messages de relance est activé par défaut. Elle ajoute parfois une seconde idée dans sa propre bulle, après sa réponse. Si tu écris ou parles, elle l'abandonne. Pas de relance pendant les tâches, les liaisons vocales ou les commentaires de jeu.", "Mainframe.cs", "Follow-up messages"),
            new Topic("Code/Husk Mode", "code/husk mode|code husk mode|code mode|husk mode|claude code hooks|codex hooks|watch claude|watch codex|coding assistant body|mode code|mode husk|hooks claude code",
                "Mainframe > Réglages > Réglages avancés > CERVEAU > Mode Code/Husk propose Non, Claude Code, Codex ou Les deux. Un choix installe ses hooks, avec sauvegarde, sans toucher aux autres. Redémarre Claude Code ; redémarre Codex et fais confiance à ses hooks. Elle suit les demandes, outils, erreurs et validations, mais tu réponds dans l'appli de code : elle n'approuve ni ne pilote jamais l'assistant. Non arrête l'écoute et retire seulement ses hooks. Avec un cerveau cloud, ce qu'elle lit part vers ce cerveau, sans les clés et tokens.", "Mainframe.cs", "Code/Husk Mode"),
            new Topic("Watch-along memory", "watch along memory|watch-along memory|watch this with me|watch a video with you|watch together|react to this|regarde ca avec moi|regardons ca|regarder ensemble|memoire du visionnage",
                "Dis « regarde ça avec moi » ou « réagis à ça ». Elle suit toute fenêtre grâce au son du PC et à des images, avec une reconnaissance hors ligne et un cerveau qui voit. Mainframe > Réglages > OBSERVER > Mémoire du visionnage garde 8 à 96 lignes en mémoire, 24 par défaut. Dis « arrête de regarder » ; 90 secondes sans son ni changement d'image l'arrêtent aussi. L'audio brut et la transcription ne sont pas enregistrés.", "Mainframe.cs", "Watch-along memory"),
            new Topic("Caresses sur la tête", "head pats|head pat|headpats|pat you|pet you|petting you|caresses|caresse moi|te caresser",
                "Mainframe > Réglages > CAPRI > Caresses sur la tête est activé par défaut. Frotte le curseur sur elle : elle sourit et dit une phrase, au plus toutes les 20 secondes, et une caresse la réveille d'une sieste.", "Mainframe.cs", "Caresses sur la tête"),
            new Topic("Hologramme", "hologram|hologram mode|unclickable|click through you|clicks go through|hologramme|mode hologramme|les clics traversent",
                "Active Mainframe > Réglages > CAPRI > Hologramme, ou dis « mode hologramme ». Elle devient translucide, et les clics traversent son corps, sa pastille et sa bulle. On ne peut plus la déplacer ni la caresser. Pour revenir, coupe l'interrupteur, décoche Hologramme dans le menu de son icône, ou dis « hologramme non ». Windows uniquement pour l'instant.", "Mainframe.cs", "Hologramme"),
            new Topic("Chaos when upset", "chaos when upset|upset chaos|chaos when youre upset|why did your clones|clones when im rude|chaos si elle est vexee|pourquoi tes clones",
                "Mainframe > Réglages > CAPRI > Chaos si elle est vexée est coupé par défaut. Activé, l'insulter peut lâcher ses clones : 1 chance sur 3 la première fois, 2 sur 3 ensuite, et toujours à la troisième. Ils se multiplient jusqu'à ce que tu maintiennes Échap. Jamais pendant une tâche, un jeu, un stream ou une discussion vocale.", "Mainframe.cs", "Chaos when upset"),
            new Topic("Desktop icon tricks", "mess up my icons|hide my icons|put my icons back|my icons moved|my icons are gone|where did my icons go|where are my icons|desktop icons|shortcuts moved|my shortcuts are gone|mes icones|cache mes icones|remets mes icones|ou sont mes icones|icones du bureau",
                "En mode chaos, elle déplace quelques icônes du bureau ou les fait disparaître, puis les remet quelques secondes plus tard. Demande « cache mes icônes » ou « remets mes icônes » quand tu veux. Elle ne touche jamais aux fichiers. Si elle se ferme en plein tour, son prochain démarrage les remet. Windows uniquement pour l'instant.", null, null),
            new Topic("Réglages avancés", "advanced settings|simple settings|more settings|all your settings|all the settings|cant find that setting|cant find the setting|where did that setting go|where did my settings go|where did the settings go|settings are missing|reglages avances|reglages simples|plus de reglages|tous tes reglages|je ne trouve pas ce reglage",
                "Mainframe > Réglages s'ouvre sur l'essentiel. Le bouton Réglages avancés, en haut à côté de Retour, montre tout ; il devient alors Réglages simples. Elle se souvient de ton choix.", "Mainframe.cs", "Réglages avancés"),
            new Topic("Extras", "extras window|extras screen|your extras|first launch picker|fenetre extras|tes extras",
                "Au premier lancement, elle ouvre EXTRAS avec quatre interrupteurs coupés : Regarder mon écran, Commenter mes jeux, Clips de mes jeux et Écouter son nom. Chacun coûte quelque chose ou utilise le micro : à toi de choisir. Terminé ferme la fenêtre. Tout se retrouve dans Réglages : OBSERVER, JEUX, CLIPS et VOIX.", "Mainframe.cs", "EXTRAS >"),
            new Topic("Language", "your language|her language|change language|speak another language|ta langue|sa langue|changer de langue|parle une autre langue",
                "Mainframe > Réglages > TOI > Langue choisit la langue de ses réponses. La changer démarre une nouvelle discussion récente. Capri est réglée en français par défaut.", "Mainframe.cs", "Language"),
            new Topic("What should she call you?", "your nickname|my nickname|change my name|what should she call you|what you call me|mon surnom|change mon nom|comment m appeler|comment tu m appelles",
                "Dans Mainframe > Réglages > TOI, saisis un surnom sous Comment doit-elle t'appeler ? et appuie sur Enregistrer. Vide-le ou écris choom pour revenir à choom. Apprends à me connaître demande ton prénom, tes pronoms et tes goûts, et les garde dans me.md.", "Mainframe.cs", "What should she call you?"),
            new Topic("Get to know me", "get to know me|your introduction|my pronouns|apprends a me connaitre|mes pronoms",
                "Mainframe > Réglages > TOI > Apprends à me connaître lance ses questions sur ton prénom, tes pronoms et tes goûts. Les réponses vont dans me.md ; dis « passe » pour sauter une question ou « stop » pour arrêter.", "Mainframe.cs", "Get to know me"),
            new Topic("Volume", "your volume|her volume|voice volume|you are too loud|you are too quiet|ton volume|son volume|volume de la voix|tu parles trop fort|tu parles trop bas",
                "Mainframe > Réglages > VOIX > Volume règle le volume de sa voix, de Muet à 150 %. Les autres applis ne changent pas. Avec un cerveau cloud, « parle plus fort » ou « moins fort » marche aussi.", "Mainframe.cs", "Volume"),
            new Topic("Pitch", "voice pitch|your pitch|her pitch|hauteur de la voix|ta hauteur",
                "Mainframe > Réglages > VOIX > Hauteur rend sa voix plus grave ou plus aiguë, de -6 à +6 demi-tons, et démarre à +2. Elle fait un essai quand tu relâches le curseur.", "Mainframe.cs", "Pitch"),
            new Topic("Speed", "voice speed|your speech speed|talk faster|talk slower|speak faster|speak slower|vitesse de la voix|parle plus vite|parle moins vite",
                "Mainframe > Réglages > VOIX > Vitesse règle sa vitesse de parole, de 0,7x à 1,5x. Elle fait un essai quand tu relâches le curseur. Avec un cerveau cloud, « parle plus vite » ou « moins vite » marche aussi.", "Mainframe.cs", "Speed"),
            new Topic("Robot", "robot voice|robotic voice|sound robotic|voix robot|voix robotique",
                "Mainframe > Réglages > VOIX > Robot ajoute un effet robotique à sa voix.", "Mainframe.cs", "Robot"),
            new Topic("Glitch", "voice glitch|glitchy voice|glitch effect|glitch de la voix|effet glitch",
                "Mainframe > Réglages > VOIX > Glitch règle l'effet de glitch dans sa voix. Ce curseur change le son, pas son animation de démarrage.", "Mainframe.cs", "Glitch"),
            new Topic("Voice layers", "voice layers|layers setting|layers switch|layer pitch|layer delay|couches de voix",
                "Mainframe > Réglages > VOIX > Couches ouvre COUCHES DE VOIX, qui ajoute deux copies plus faibles de sa voix, coupées par défaut. Chacune a sa hauteur et son délai. L'écouter fait un essai et Par défaut rétablit les valeurs.", "Mainframe.cs", "Voice layers"),
            new Topic("Sounds", "your sound effects|her sound effects|sound effects volume|tes effets sonores|volume des effets",
                "Mainframe > Réglages > VOIX > Sons règle ses effets sonores. Le volume de la voix a son propre curseur.", "Mainframe.cs", "Sounds"),
            new Topic("Typing sound", "typing sound|typing noise|typing clicks|son de frappe|bruit de clavier",
                "Mainframe > Réglages > VOIX > Son de frappe active ou coupe le léger bruit de clavier pendant ses réponses écrites.", "Mainframe.cs", "Typing sound"),
            new Topic("Free voice in games", "free voice in games|voix gratuite en jeu",
                "Mainframe > Réglages > VOIX > Voix gratuite en jeu apparaît quand la voix HD est choisie, et est coupée par défaut. Activée, un jeu ou une vidéo en plein écran fait passer à la voix gratuite pour libérer la carte graphique.", "Mainframe.cs", "Free voice in games"),
            new Topic("Sa voix dans ton micro", "her voice in your mic|your voice in my mic|your voice into my mic|you in my mic|vb cable|cable output|cable input|virtual mic|virtual cable|friends hear you|steam streaming microphone|steam mic|set up vb|install vb|sa voix dans mon micro|ta voix dans mon micro|micro virtuel|vb cable|cable virtuel|mes amis t entendent",
                "Mainframe > Réglages > VOIX > Sa voix dans ton micro est coupé par défaut. Activé, elle envoie ton micro et sa voix dans un micro virtuel, que tu choisis comme micro dans ton appli d'appel ou Roblox : Steam Streaming Microphone si présent, ou CABLE Output une fois VB-CABLE installé. Sans les deux, Configurer ouvre l'installeur de VB-CABLE : autorise-le, clique sur Install Driver, puis redémarre ton PC. VB-CABLE est un donationware de vb-cable.com. Un casque évite que ton micro l'entende deux fois.", "Mainframe.cs", "Sa voix dans ton micro"),
            new Topic("Sa voix dans ton micro: loudness", "her voice in your mic loudness|how loud she is in your mic|louder in my mic|quieter in my mic|voice volume on calls|friends cant hear you|friends hear you quietly|volume dans mon micro|plus fort dans mon micro|moins fort dans mon micro",
                "Mainframe > Réglages > VOIX > Sa voix dans ton micro : volume va de 50 % à 200 %, par pas de 10 %, et démarre à 100 %. Cela change sa voix dans le micro virtuel, pas tes haut-parleurs ni ton micro.", "Mainframe.cs", "Sa voix dans ton micro: loudness"),
            new Topic("Mini-lecteur", "mini player|music player|your player|mini lecteur|lecteur de musique|ton lecteur",
                "Mainframe > Réglages > MOUVEMENTS > Mini-lecteur montre la chanson signalée par Windows, avec des commandes et une place où elle s'assoit. Il se masque en plein écran, sauf si Mini-lecteur en plein écran est activé dans HABITUDES.", "Mainframe.cs", "Mini-lecteur"),
            new Topic("Wander", "your wandering|her wandering|wander around|wander off|stop wandering|dont wander|why dont you move|why arent you moving|tes balades|balade toi|arrete de te balader|arreter de te balader|pourquoi tu ne bouges pas",
                "Mainframe > Réglages > MOUVEMENTS > Balades propose Non, Rarement, Parfois, Souvent ou Agitée, et démarre sur Rarement. Clic droit > Déplacer > Se balader l'active aussi. Elle se balade quand elle est au repos et que tu es actif, et reste où tu l'as assise.", "Mainframe.cs", "Wander"),
            new Topic("Sleep", "your sleep|her sleep|when do you sleep|when you fall asleep|go to sleep|wake you up|ton sommeil|quand dors tu|va dormir|reveille toi",
                "Mainframe > Réglages > MOUVEMENTS > Sommeil choisit le temps de calme avant qu'elle s'endorme, ou Jamais. Les tâches, appels, discussions vocales et le mode jeu la gardent éveillée. Bouger la souris la réveille, et un clic ou une caresse réveille aussi une sieste demandée. Pendant une tâche, elle la termine d'abord.", "Mainframe.cs", "Sleep"),
            new Topic("Pop-ins", "pop ins|popins|how often you visit|how often you pop in|apparitions|a quelle frequence tu viens",
                "Mainframe > Réglages > MOUVEMENTS > Apparitions règle le rythme : Rarement 20 à 40 minutes, Parfois 8 à 20, Souvent 3 à 8. L'option du même nom dans l'en-tête MOUVEMENTS fixe ton minimum et ton maximum. Prendre de mes nouvelles active ou coupe ces visites.", "Mainframe.cs", "Pop-ins"),
            new Topic("Dance to music", "dance to music|your dancing|why dont you dance|stop dancing|danser en musique|pourquoi tu ne danses pas|arrete de danser",
                "Mainframe > Réglages > MOUVEMENTS > Danser en musique active ou coupe la danse quand de la musique est détectée. Le même interrupteur est dans clic droit > Déplacer. Windows doit signaler le média comme de la musique ; Déplacer > Danser la fait danser directement.", "Mainframe.cs", "Dance to music"),
            new Topic("Glitch in at startup", "glitch in at startup|startup animation|your entrance|arrivee en glitch|animation de demarrage",
                "Mainframe > Réglages > MOUVEMENTS > Arrivée en glitch au démarrage active ou coupe son effet d'arrivée. Cela ne décide pas si elle démarre avec Windows.", "Mainframe.cs", "Glitch in at startup"),
            new Topic("Hotkeys", "hotkeys|hot keys|your shortcuts|keyboard shortcuts|change your keys|keybinds|ctrl alt|raccourcis clavier|tes touches|change tes touches|ctrl alt",
                "Mainframe > Réglages > TOUCHES : clique sur un raccourci, appuie sur les nouvelles touches et confirme ; Annuler garde l'ancien. Par défaut : Ctrl + Alt + V pour parler, N pour le micro, T pour écrire, H pour masquer, P pour le silence et G pour le mode jeu. Appuyer pour parler n'a pas de touche au départ.", "Mainframe.cs", "TOUCHES >"),
            new Topic("Push to talk key", "push to talk key setting|bind push to talk|push to talk button|touche appuyer pour parler",
                "Mainframe > Réglages > TOUCHES > Appuyer pour parler te permet de maintenir une touche ou un bouton de souris pour lui parler, même en jeu. Clique dessus, appuie sur la touche, confirme, ou Non pour l'effacer. La touche garde sa fonction dans le jeu.", "Mainframe.cs", "Push to talk: not set"),
            new Topic("Quiet", "quiet mode|quiet key|make you quiet|keep you quiet|stop interrupting|stop talking on your own|mode silence|touche de silence|tais toi|arrete de m interrompre",
                "Ctrl + Alt + P active ou coupe le silence ; Mainframe > Réglages > TOUCHES change la touche. Elle cesse ses coups d'œil, rappels et plaisanteries spontanés, mais répond toujours quand tu lui parles. Réappuie pour reprendre, sans changer tes autres réglages.", "Mainframe.cs", "Quiet: Ctrl + Alt + P"),
            new Topic("Size", "your size|her size|make you bigger|make you smaller|make her bigger|make her smaller|resize you|resize capri|ta taille|sa taille|fais toi plus grande|fais toi plus petite",
                "Mainframe > Réglages > CAPRI > Taille change la taille de Capri, de Minuscule à Géante. Clic droit > Taille propose aussi des choix ; la taille du Mainframe est un réglage à part. Avec un cerveau cloud, « fais-toi plus grande » marche aussi.", "Mainframe.cs", "Size"),
            new Topic("Mainframe size", "mainframe size|settings too small|mainframe too small|mainframe bigger|virtual super resolution|dsr|taille du mainframe|mainframe trop petit",
                "Mainframe > Réglages > CAPRI > Taille du Mainframe met la fenêtre à l'échelle, en Auto ou de 100 % à 200 %. Auto compense quand Virtual Super Resolution ou DSR agrandit le bureau par rapport à l'écran.", "Mainframe.cs", "Mainframe size"),
            new Topic("Prendre de mes nouvelles", "check in on me|pop in on me|your check ins|you keep popping up|keep asking for attention|why are you asking for attention|prendre de mes nouvelles|tes rappels|pourquoi tu reviens sans cesse",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles regroupe les visites, encouragements et rappels : eau, étirements, yeux, sommeil, repas, pauses de jeu et ennui. Les détails sont juste en dessous dans Réglages avancés ; le couper garde tes choix.", "Mainframe.cs", "Prendre de mes nouvelles"),
            new Topic("Settings ignored", "ignoring my settings|disobeying my settings|not following my settings|i turned that off|i switched that off|i disabled that|tu ignores mes reglages|je l ai desactive|j ai coupe ca",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles arrête tous les rappels, OBSERVER > Regarder mon écran arrête les commentaires d'écran, et JEUX > Commentaires > Non arrête les commentaires de jeu hors mode jeu. Le silence l'empêche de parler d'elle-même, mais elle te répond toujours.", "Capri.cs", "check_ins"),
            new Topic("Always on top", "always on top|keep you on top|behind my windows|behind other windows|toujours au premier plan|derriere mes fenetres",
                "Mainframe > Réglages > CAPRI > Toujours au premier plan la garde au-dessus des fenêtres normales. Une appli en plein écran exclusif peut quand même la couvrir ; le mode sans bordure ou fenêtré la laisse visible.", "Mainframe.cs", "Always on top"),
            new Topic("Regarder mon écran", "watch my screen|watching my screen|your screen watching|see my screen|read my screen|screen watching|cant you see|cannot you see|regarde mon ecran|regarder mon ecran|vois tu mon ecran|lis mon ecran",
                "Mainframe > Réglages > OBSERVER > Regarder mon écran active des commentaires occasionnels ; Tous les règle l'intervalle. Les jeux ont leur propre réglage dans JEUX. Il faut un cerveau capable de voir. Elle ignore les captures inchangées ou vides, le silence met ses coups d'œil en pause, et lui demander de regarder reste une action à part.", "Mainframe.cs", "Regarder mon écran"),
            new Topic("Screens to watch", "screens to watch|which monitor you watch|choose a monitor to watch|watch one monitor|watch my other screen|watch all screens|ecrans a regarder|quel ecran tu regardes",
                "Mainframe > Réglages > OBSERVER > Écrans choisit un écran pour Regarder mon écran ; Tous les écrans suit l'appli au premier plan. Un écran absent reste enregistré, et tous les écrans sont regardés jusqu'à son retour.", "Mainframe.cs", "Screens"),
            new Topic("Remember what she sees", "remember what she sees|remember what you see|keep her caught up|keep you caught up|caught up setting|silent glances|se souvenir de ce qu elle voit",
                "Mainframe > Réglages > OBSERVER > Se souvenir de ce qu'elle voit fonctionne avec Regarder mon écran. Même sans commentaire, elle garde ce qu'elle a vu pendant trois heures, pour que tu puisses lui en parler. Cela consomme un peu plus ton API.", "Mainframe.cs", "Remember what she sees"),
            new Topic("Every", "glance interval|time between glances|how often you look|how often you watch|intervalle des coups d oeil|a quelle frequence tu regardes",
                "Mainframe > Réglages > OBSERVER > Tous les règle l'écart entre deux coups d'œil, de 10 secondes à 10 minutes. Regarder mon écran doit être actif ; elle peut sauter un coup d'œil si l'écran n'a pas changé ou si elle est occupée.", "Mainframe.cs", "Every"),
            new Topic("Stay on my model", "stay on my model|switching models|changing brains by yourself|model fallback|rester sur mon modele",
                "Mainframe > Réglages > CERVEAU > Rester sur mon modèle garde le modèle NVIDIA choisi quand il bloque : elle signale le problème au lieu d'en essayer un autre.", "Mainframe.cs", "Stay on my model"),
            new Topic("Replies", "your replies|her replies|reply length|longer replies|shorter replies|long answers|short answers|tes reponses|longueur des reponses|reponses plus longues|reponses plus courtes",
                "Mainframe > Réglages > CERVEAU > Réponses choisit Court, Moyenne ou Long. Cela change ce qu'elle dit, tandis que Bulle change la durée d'affichage.", "Mainframe.cs", "Replies"),
            new Topic("Max reply", "max reply|maximum reply|reply tokens|token limit|reply limit|reponse max|limite de tokens",
                "Mainframe > Réglages > CERVEAU > Réponse max propose Auto, 256, 2K ou 8K tokens. Auto suit le réglage Réponses ; les tâches gardent plus de place si besoin. Plus de place ne rend pas chaque réponse longue.", "Mainframe.cs", "Max reply"),
            new Topic("Memory", "your memory setting|her memory setting|recent chat|chat context|forget our conversation|ta memoire|discussion recente|oublie notre conversation",
                "Mainframe > Réglages > CERVEAU > Mémoire choisit Court, Moyenne ou Long pour 8, 16 ou 32 messages récents. Cette discussion peut survivre aux redémarrages pendant une semaine ; Retenir nos discussions et me.md gèrent la mémoire longue.", "Mainframe.cs", "Memory"),
            new Topic("Memory size", "memory size|context size|context window|context tokens|taille de la memoire|taille du contexte",
                "Mainframe > Réglages > CERVEAU > Taille de la mémoire règle le contexte en tokens pour Ollama et son cerveau intégré : Auto, 4K, 8K, 16K, 32K ou 64K. Cela ne change ni un cerveau cloud ni un autre serveur local.", "Mainframe.cs", "Memory size"),
            new Topic("Bubble", "bubble setting|your bubble|your bubbles|her bubble|bubble duration|how long replies stay|text disappears|reglage de la bulle|ta bulle|duree de la bulle|le texte disparait",
                "Mainframe > Réglages > CERVEAU > Bulle choisit Rapide, Normale ou Long pour la durée d'affichage de sa bulle. La durée suit aussi la longueur de la réponse.", "Mainframe.cs", "Bubble"),
            new Topic("Custom model name", "custom model name|custom brain|another ollama model|nom du modele perso|autre modele ollama",
                "Mainframe > Réglages > CERVEAU > Nom du modèle perso accepte un modèle Ollama déjà téléchargé, un identifiant de ton serveur local ou un modèle de ta clé. Saisis-le et appuie sur Utiliser ; rien n'est installé pour toi.", "Mainframe.cs", "Custom model name"),
            new Topic("Local servers", "local server|local servers|local server address|lm studio|use jan|jan server|jan brain|connect jan|koboldcpp|llama cpp|serveur local|serveurs locaux|adresse du serveur local|lm studio",
                "LM Studio, Jan, KoboldCpp et llama.cpp peuvent fournir son cerveau NORMAL. Lance le serveur avec un modèle chargé, choisis NORMAL dans Mainframe > CERVEAU, appuie sur Actualiser et choisis le modèle. Pour une autre adresse, utilise Mainframe > Réglages > CERVEAU > Adresse du serveur local, par exemple http://127.0.0.1:1234.", "Mainframe.cs", "Local server address"),
            new Topic("Lancer avec Windows", "start with windows|run at startup|launch at startup|start automatically|lancer avec windows|demarrer avec windows|demarrage automatique",
                "Mainframe > Réglages > HABITUDES > Lancer avec Windows contrôle le lancement automatique pour ton compte Windows. La copie installée l'active une fois au premier lancement, et le couper reste valable ; le réglage du Gestionnaire des tâches y est aussi reflété.", "Mainframe.cs", "Lancer avec Windows"),
            new Topic("Show on Discord", "show on discord|playing capri|discord status|discord activity|afficher sur discord|statut discord",
                "Mainframe > Réglages > HABITUDES > Afficher sur Discord montre une courte activité et le temps écoulé tant que Discord tourne sur ce PC. Ni titres de chansons ni contenu des tâches.", "Mainframe.cs", "Show on Discord"),
            new Topic("Use her cursor", "use her cursor|use your cursor|your pink pointer|your cursor setting|utiliser son curseur|ton curseur",
                "Mainframe > Réglages > HABITUDES > Utiliser son curseur laisse les recherches YouTube et le volume Spotify passer par son pointeur visible, si un cerveau et l'autorisation de souris sont disponibles. Coupé, elle utilise les outils rapides ; les tâches explicites à la souris restent possibles.", "Mainframe.cs", "Use her cursor"),
            new Topic("Obsidian vault", "memory vault|obsidian|my vault|in obsidian|to obsidian|obsidian vault|coffre obsidian|mon coffre|obsidian",
                "Mainframe > Réglages > HABITUDES > Coffre Obsidian permet de choisir un de tes coffres, ou une note .md. Ses notes sur toi vont alors dans une note Capri Memories, lisible dans Obsidian. Ses transcriptions et memories.md restent dans son dossier. Ses propres notes la remettent sur me.md.", "Mainframe.cs", "Obsidian vault"),
            new Topic("Retenir nos discussions", "remember our chats|memories md|chat transcripts|open her memories|your memories|retenir nos discussions|tes souvenirs|transcriptions",
                "Mainframe > Réglages > HABITUDES > Retenir nos discussions garde des transcriptions quotidiennes et résume les anciennes discussions dans memories.md, sur ce PC. Ouvrir ses souvenirs les modifie dans le Mainframe. Couper ce réglage arrête l'écriture et la lecture sans rien supprimer. Avec un cerveau cloud, les souvenirs pertinents lui sont envoyés comme contexte.", "Mainframe.cs", "Retenir nos discussions"),
            new Topic("Perch", "perch fps|perch frame rate|perch setting|your animation speed|perchoir|images par seconde",
                "Mainframe > Réglages > HABITUDES > Perchoir choisit les images par seconde quand elle est assise sur une fenêtre. Au-delà de 30, le mouvement est plus fluide et utilise plus de processeur.", "Mainframe.cs", "Perch"),
            new Topic("Garder le PC éveillé pour les tâches", "keep pc awake|keep my pc awake|keep the pc awake|keep screen on|keep my screen on|keep the screen on|keep screen awake|keep awake for jobs|sleep during jobs|laptop lid|garder le pc eveille|garder l ecran allume",
                "Mainframe > Réglages > HABITUDES > Garder le PC éveillé pour les tâches est activé par défaut. Le PC et l'écran restent actifs pendant une tâche, une attente de réponse ou une surveillance. Les réglages d'alimentation de Windows ne changent pas, et un ordinateur portable refermé se met quand même en veille.", "Mainframe.cs", "Garder le PC éveillé pour les tâches"),
            new Topic("Water", "water check in|water check ins|water setting|water reminder setting|rappel eau|rappel d eau|boire de l eau",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Eau active ou coupe ses rappels d'hydratation, toutes les 60 minutes par défaut. Modifier les rappels change ses phrases.", "Care.cs", "Water"),
            new Topic("Stretch", "stretch check in|stretch check ins|stretch setting|stretch reminder setting|rappel etirements|etirements",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Étirements active ou coupe ses rappels, toutes les 90 minutes par défaut. Modifier les rappels change ses phrases.", "Care.cs", "Stretch"),
            new Topic("Look away", "look away setting|eye break setting|eye check ins|rest your eyes setting|regarder ailleurs|pause des yeux",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Regarder ailleurs te rappelle de quitter l'écran des yeux 20 secondes, toutes les 25 minutes par défaut. Modifier les rappels change ses phrases.", "Care.cs", "Look away"),
            new Topic("Sleep nudge", "sleep nudge|bedtime nudge|bedtime setting|rappel de sommeil|heure du coucher",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Rappel de sommeil donne un rappel par nuit, à 1 h par défaut. Ce rappel te concerne : MOUVEMENTS > Sommeil concerne sa propre sieste.", "Care.cs", "Sleep nudge"),
            new Topic("Meals", "meals setting|meal check in|meal check ins|meal reminder setting|rappel repas|repas",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Repas te rappelle de manger si tu es là depuis avant midi. Les rappels commencent à midi et reviennent toutes les six heures par défaut.", "Care.cs", "Meals"),
            new Topic("Game break", "game break setting|game break check in|game break check ins|pause de jeu",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Pause de jeu donne un rappel après deux heures dans un jeu détecté, par défaut. Changer de jeu relance le compteur, et elle attend d'être libre et hors plein écran.", "Care.cs", "Game break"),
            new Topic("Bored", "bored setting|bored check in|bored check ins|boredom check in|stop saying youre bored|ennui|arrete de dire que tu t ennuies",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Ennui gère ses demandes d'attention, après 60 minutes par défaut. La remarquer relance l'attente ; l'ignorer allonge les suivantes. Modifier les rappels contient Ennui, Ennui remarqué et Ennui ignoré.", "Care.cs", "Bored"),
            new Topic("Edit check-in lines", "edit check in lines|check ins md|change check in lines|check in wording|modifier les rappels|phrases des rappels",
                "Mainframe > Réglages > CAPRI > Prendre de mes nouvelles > Modifier les rappels ouvre check-ins.md dans le Bloc-notes. Modifie les lignes et enregistre, ou retire toutes celles d'un titre pour couper ce rappel. Plus de dix minutes d'absence relancent les compteurs.", "Mainframe.cs", "Edit check-in lines"),
            new Topic("Comments", "your game comments|game commentary setting|comments setting|commentaires de jeu|tes commentaires",
                "Mainframe > Réglages > JEUX > Commentaires propose Non, Événements seuls ou Bavarde, indépendamment de Regarder mon écran. Pour aucun commentaire, coupe Commentaires et le mode jeu. Ils utilisent les observations d'écran, le niveau sonore et certains journaux de jeu.", "Mainframe.cs", "Comments"),
            new Topic("Live reactions", "live reactions|live reaction|reaction feed|reactions feed|hype lines|hype feed|reactions en direct",
                "Mainframe > Réglages > JEUX > Réactions en direct est activé par défaut. Dans un jeu en plein écran, quand il se passe quelque chose, son cerveau écrit une courte phrase à droite de l'écran, si elle arrive en 2,5 secondes environ. Quatre au plus s'empilent et chacune s'efface après 4 secondes. Elle en dit au plus une toutes les 8 secondes ; La rendre muette les garde en texte. Pas d'affichage en plein écran exclusif.", "Mainframe.cs", "Live reactions"),
            new Topic("Masquer en plein écran", "hide in full screen|hide in fullscreen|disappear in fullscreen|hide over videos|masquer en plein ecran|disparaitre en plein ecran",
                "Mainframe > Réglages > HABITUDES > Masquer en plein écran est coupé par défaut. Activé, elle se cache quand la fenêtre au premier plan remplit l'écran, puis revient. Un masquage manuel reste actif, et le mode jeu reste prioritaire.", "Mainframe.cs", "Masquer en plein écran"),
            new Topic("Mini-lecteur over full screen", "mini player over full screen|mini player over fullscreen|player over games|music over fullscreen|mini lecteur en plein ecran",
                "Mainframe > Réglages > HABITUDES > Mini-lecteur en plein écran est coupé par défaut. Activé, son mini-lecteur reste au-dessus des fenêtres sans bordure, mais pas en plein écran exclusif.", "Mainframe.cs", "Mini-lecteur over full screen"),
            new Topic("Explorer pendant mon absence", "explore while im away|explore while i'm away|afk exploring|scroll while away|explore my screen|explorer pendant mon absence|defiler pendant mon absence",
                "Mainframe > Réglages > HABITUDES > Explorer pendant mon absence est coupé par défaut. Après le délai, cinq minutes par défaut, elle bouge son propre pointeur et fait défiler une page de navigateur ou de lecture. Elle ne clique, n'écrit, n'ouvre et ne ferme jamais rien. Jeux, plein écran et champs de mot de passe sont exclus. Toute action de ta part l'arrête, et elle s'arrête après une demi-heure.", "Mainframe.cs", "Explorer pendant mon absence"),
            new Topic("Explore after", "explore after|away time|afk time|exploring delay|explorer apres|delai d absence",
                "Mainframe > Réglages > HABITUDES > Explorer après règle le temps d'absence avant Explorer pendant mon absence : 1, 2, 5, 10, 15 ou 30 minutes, 5 par défaut.", "Mainframe.cs", "Explore after"),
            new Topic("Mode jeu", "gaming mode|game mode|gamer mode|gaming mod|game mod|gaming mood|your gaming hud|your game overlay|turn on the hud|activate the hud|enable the hud|start the hud|move the hud|move your hud|reset the hud|hud position|gaming mode key|mode jeu|mode gaming|touche du mode jeu",
                "Dis « Hey Capri, active le mode jeu » ou utilise Mainframe > Réglages > JEUX ; la touche par défaut est Ctrl + Alt + G. Il est activé par défaut : un jeu au premier plan le lance sans rien dire, et le couper le garde coupé. Elle reste sur ton bureau et réagit au jeu jusqu'à sa fermeture. Il marche mieux avec INTELLIGENCE MAX. Le mode jeu ne lui demande pas de jouer à ta place.", "Mainframe.cs", "Mode jeu"),
            new Topic("Clips de mes jeux", "clip my games|clip my game|clip a game|game clips|saved clips|clips saved|your clips|clip that|scroll that bd|record my games|clips de mes jeux|clip ca|mes clips|enregistre mes jeux",
                "Mainframe > Réglages > CLIPS > Clips de mes jeux est coupé par défaut. Activé, il garde en mémoire jusqu'aux deux dernières minutes de vidéo et de son d'un jeu détecté, tant qu'il est au premier plan. Dis « clip ça » ou précise une durée. Les clips sont des MP4 dans le dossier choisi, Vidéos\\Capri FR Clips par défaut. Chaque enregistrement affiche CLIP ENREGISTRÉ ; clique dessus pour le voir.", "Mainframe.cs", "Clips de mes jeux"),
            new Topic("Enregistrer les grands moments", "save big moments|automatic clips|auto clips|automatic clipping|clip moments|big moments|enregistrer les grands moments|clips automatiques",
                "Mainframe > Réglages > CLIPS > Enregistrer les grands moments est coupé par défaut et active aussi Clips de mes jeux. Ses commentaires de jeu décident alors si un moment mérite d'être gardé, à ta durée par défaut. Les commentaires doivent tourner. Il n'y a pas de limite au nombre de clips, et elle ne supprime pas les anciens : surveille la place disque.", "Mainframe.cs", "Enregistrer les grands moments"),
            new Topic("Default length", "default length|clip length|clip duration|recording duration|duree par defaut|duree du clip",
                "Mainframe > Réglages > CLIPS > Durée par défaut choisit 15, 30, 45, 60, 90 ou 120 secondes, 30 par défaut. Une demande précise la remplace, dans la limite de ce qui est en mémoire.", "Mainframe.cs", "Default length"),
            new Topic("Frame rate", "frame rate|clip fps|recording fps|clip frame rate|images par seconde du clip|fps des clips",
                "Mainframe > Réglages > CLIPS > Images par seconde vise 15, 24, 30 ou 60 i/s, 30 par défaut. Une capture lente ou le budget de 240 Mo de mémoire peuvent réduire ce nombre.", "Mainframe.cs", "Frame rate"),
            new Topic("Resolution", "clip resolution|recording resolution|clip quality|resolution|resolution des clips|qualite des clips",
                "Mainframe > Réglages > CLIPS > Résolution choisit Auto ou jusqu'à 480p, 720p ou 1080p, sans agrandir l'image. Auto démarre à 1080p maximum et peut baisser pour rester fluide.", "Mainframe.cs", "Resolution"),
            new Topic("Video bitrate", "video bitrate|clip bitrate|recording bitrate|bitrate|debit video|debit des clips",
                "Mainframe > Réglages > CLIPS > Débit vidéo choisit Auto ou un débit H.264 moyen de 2, 4, 8, 12 ou 20 Mbit/s. Plus il est élevé, plus les fichiers sont gros. Ce n'est pas une limite garantie.", "Mainframe.cs", "Video bitrate"),
            new Topic("Save folder", "clip folder|clips folder|save folder|clip destination|where are clips saved|where are clips stored|dossier des clips|ou sont enregistres les clips",
                "Mainframe > Réglages > CLIPS > Choisir un dossier choisit le dossier des prochains MP4, Vidéos\\Capri FR Clips par défaut. Les anciens clips restent où ils sont. Ouvrir le dossier des clips ouvre le dossier choisi.", "Mainframe.cs", "Save folder"),
            new Topic("Mute her", "mute her|mute capri|mute button|mute you|stop talking on your own|la rendre muette|rends toi muette",
                "Mainframe > Réglages > JEUX > La rendre muette l'empêche de parler d'elle-même, mais elle répond quand tu lui parles. C'est comme la touche de silence, Ctrl + Alt + P. Ses réactions en jeu restent affichées en texte.", "Mainframe.cs", "Mute her"),
            new Topic("Fast hands in games", "fast hands|haiku in games|games on haiku|play on haiku|why haiku|hands brain|cheaper games|faster in games|mains rapides|jeux sur haiku",
                "Mainframe > Réglages > JEUX > Mains rapides en jeu est activé par défaut. Avec une clé Claude, une tâche de jeu tourne sur Claude Haiku 5.5, plus rapide et bien moins cher. Si elle bloque, elle demande à ton cerveau, ou à Claude Sonnet 5.5 si c'est déjà Haiku. La discussion reste sur ton cerveau.", "Mainframe.cs", "Fast hands in games"),
            new Topic("Mini games", "mini games|mini game|minigames|tic tac toe|tictactoe|connect four|connect 4|rock paper scissors|desktop boards|mini jeux|mini jeu|morpion|puissance 4|puissance quatre|pierre feuille ciseaux",
                "Mainframe > Réglages > JEUX > Jouer propose échecs, Puissance 4, morpion et pierre-feuille-ciseaux, même sans cerveau connecté. Clique sur un coup ou dis-lui une case, une colonne ou une main. Morpion : touches 1-9. Puissance 4 : 1-7. Pierre-feuille-ciseaux : R, P ou S. N relance et Échap ferme.", "Hands.cs", "mini_game"),
            new Topic("Game controls", "game controls|teach you controls|teach game controls|game instructions|your saved controls|commandes de jeu|apprends les commandes",
                "Mainframe > Réglages > JEUX > Commandes ouvre un éditeur pour les commandes d'un jeu ; utilise le titre de sa fenêtre comme nom. Tu peux aussi dire « retiens les commandes de tel jeu : tire avec les flèches ». Elles sont dans games/<jeu>.controls.md, et chaque tâche de jeu les lit d'abord.", "Hands.cs", "game_notes"),
            new Topic("Chess", "chess|chess board|chess difficulty|echecs|jouer aux echecs|difficulte des echecs",
                "Demande « joue aux échecs », en facile, normal ou difficile. Tu as les blancs, sauf si tu dis le contraire. Clique sur une pièce puis une case, ou fais-la glisser. Annuler, nouvelle partie, indice et abandon marchent au texte, à la voix ou par boutons. Pas besoin de cerveau ni de clé. Fermer efface la position sans demander.", "ChessBoard.cs", "CHESS"),
            new Topic("Drawing on your screen", "drawing on your screen|draw on my screen|draw on your screen|drawing on my screen|drawing on screen|your drawing|her drawing|your ink|dessine sur mon ecran|ecris sur mon ecran|ton dessin|efface ton dessin",
                "Demande-lui de dessiner et elle le fait en rose sur ton écran : « dessine un chat », « entoure le bouton Démarrer ». Les clics traversent l'encre. Le dessin reste jusqu'à ce que tu dises « efface ton dessin », maintiennes Échap ou en demandes un nouveau. Dis « ajoute » pour garder l'existant.", null, null),
            new Topic("Copy a reply", "copy a reply|copy your reply|copy her reply|copy what you said|copy your message|copy your text|copier une reponse|copie ta reponse",
                "Fais un clic droit sur sa bulle pour copier toute la réponse. La bulle affiche alors > sys : copié. Un clic gauche la ferme.", null, null),
            new Topic("Handing her a file", "handing her a file|hand you a file|give you a file|drop a file|drag a file|attach a file|read a file|read files|files can you read|files do you read|file types|file formats|read a pdf|read a spreadsheet|donner un fichier|lis un fichier|lire un fichier|quels fichiers peux tu lire|lire un pdf",
                "Fais glisser un fichier sur Capri, ou demande de le lire avec son chemin complet. Elle lit le texte et le code, Word .docx, Excel .xlsx, les PDF par l'OCR de Windows, et les images PNG, JPG, GIF, BMP et WebP. Son cerveau choisi répond à partir du contenu, qui part vers lui s'il est dans le cloud.", "Hands.cs", "read_file"),
            new Topic("Finding files", "find files|find a folder|find game folders|finding files|file search|trouve un fichier|trouver un dossier|chercher un fichier",
                "Demande un fichier ou un dossier par son nom ou une description. Elle cherche dans les noms, d'abord dans tes dossiers personnels et de jeux, puis sur tes disques. Les résultats montrent les chemins complets. Dis « montre-le » pour le sélectionner, ou « stop » pour annuler. Elle ne lit pas le contenu des fichiers pour chercher.", "Hands.cs", "find_file"),
            new Topic("Website nicknames", "website nicknames|website aliases|remember a website|name a website|surnoms de sites|retiens ce site|nomme ce site",
                "Dis « retiens ce site comme pommes ». Ensuite, « ouvre pommes » l'ouvre. « Quels sites connais-tu » les liste, et « oublie le site pommes » en retire un. Ils sont dans websites.json, à côté de memories.md.", "Hands.cs", "website_nickname"),
            new Topic("Moves", "your moves|move you|move capri|ride cursor|hop off|sit here|hang here|sit on windows|bring here|bring to this screen|tes mouvements|deplace toi|sur le curseur|assieds toi ici|accroche toi ici",
                "Fais glisser Capri pour la placer, double-clique pour la mettre sur le curseur, ou clic droit > Déplacer. S'asseoir sur les fenêtres la laisse se poser sur leurs bords. Mainframe > CAPRI contient Amener ici et Masquer Capri, et un double-clic sur son icône la ramène.", "Capri.cs", "Sit on windows"),
            new Topic("Hide Capri", "hide capri button|show capri button|masquer capri|afficher capri",
                "Mainframe > CAPRI > Masquer Capri la cache et arrête ses mouvements, sa discussion et sa voix. Le bouton devient Afficher Capri. Elle continue de tourner.", "Mainframe.cs", "Hide Capri"),
            new Topic("Bring here", "bring here button|bring capri here button|amener ici",
                "Mainframe > CAPRI > Amener ici la réveille, la fait descendre du curseur et la libère de son siège. Elle apparaît près du coin inférieur droit de l'écran du curseur.", "Mainframe.cs", "Bring here"),
            new Topic("Ride cursor", "ride cursor button|hop off button|sur le curseur|monter sur le curseur|descendre",
                "Mainframe > CAPRI > Sur le curseur la fait suivre ta souris. Les clics la traversent. Le bouton devient Descendre, et elle revient alors à sa position.", "Mainframe.cs", "Ride cursor"),
            new Topic("Notes", "me md|your notes|her notes|open her notes|edit your notes|remember things about me|me md|tes notes|ses notes|ouvrir ses notes",
                "Mainframe > Réglages > TOI > Ouvrir ses notes modifie me.md dans le Mainframe, ou sa note dans ton coffre Obsidian. Enregistrer écrit tes changements ; Ignorer + recharger relit le fichier. Dis-lui de retenir ou d'oublier quelque chose, ou modifie le fichier.", "Mainframe.cs", "Ouvrir ses notes"),
            new Topic("personality.md", "personality md|edit your personality|change your personality|personality file|personality md|change ta personnalite|modifier ta personnalite",
                "Mainframe > Réglages > TOI > Sa personnalité > Modifier personality.md l'ouvre dans le Bloc-notes. Réécris sa façon d'être, enregistre, et sa prochaine réponse en tient compte. Ses règles de sécurité restent dans son code et passent toujours après. Rétablir l'origine garde ta version dans personality.backup.md.", "Mainframe.cs", "Edit personality.md"),
            new Topic("Reminders", "your reminders|set a reminder|set reminders|reminder setting|do you have reminders|can you set reminders|can you remind me later|can you remind me tomorrow|how do reminders work|can you set a timer|can you set timers|do you have timers|do you have a timer|how do timers work|tes rappels|mets un rappel|rappelle moi|peux tu me rappeler|mets un minuteur|comment marchent les rappels",
                "Demande « rappelle-moi dans 20 minutes de sortir la pizza » ou « le 5 décembre à 9 h d'appeler maman ». Sans heure, c'est 9 h. Elle utilise l'horloge de ton PC et les garde dans reminders.json. « Liste mes rappels » et « annule le rappel » les gèrent. Les minuteurs sont aussi des rappels. Capri doit tourner pour les délivrer ; ceux en retard sont annoncés à son démarrage.", "Hands.cs", "remind"),
            new Topic("Screen watches", "watch for something|watch something for me|your screen watches|how do screen watches work|can you keep an eye on|surveille pour moi|dis moi quand|comment marchent les surveillances",
                "Demande « dis-moi quand le téléchargement est fini » ou « préviens-moi si le boss apparaît ». Avec un cerveau qui voit, elle vérifie l'écran environ toutes les trois secondes. Elle reste silencieuse jusqu'à la détection. « Que surveilles-tu » et « arrête de surveiller » les gèrent. Elles s'arrêtent à sa fermeture.", "Lookout.cs", "watch_for"),
            new Topic("Routines", "your routines|saved routines|routine setting|save a routine|set up a routine|how do routines work|routine phrases|tes routines|enregistre une routine|comment marchent les routines",
                "Dis « quand je dis concentration, ouvre le Bloc-notes » pour enregistrer une routine. Dire « concentration » lance ensuite ces étapes. « Liste mes routines » et « oublie la routine » les gèrent. Elles sont sur ce PC, dans routines.json.", "Hands.cs", "routine"),
            new Topic("Stop a job", "stop a job|stop your job|stop you working|cancel a job|cancel your job|escape key|esc key|stop your task|arrete la tache|arreter une tache|annule la tache|touche echap",
                "Appuie sur Échap pendant une tâche à la souris et au clavier pour l'arrêter. Bouger ta souris ne l'annule pas.", "Capri.cs", "Esc"),
            new Topic("Updates", "your updates|update you|update capri|check for updates|restart to update|your version|mises a jour|verifier les mises a jour|ta version",
                "Les mises à jour automatiques sont désactivées dans Capri FR : elle ne vérifie ni ne télécharge rien toute seule, y compris les correctifs de sécurité de Capri. Pour mettre à jour, installe la dernière version depuis la page GitHub de Capri FR.", "Mainframe.cs", "Check for updates"),
            new Topic("Data folder", "your data|her data|data folder|where are your files|where do you store|localappdata|settings json|chat json|tes donnees|dossier de donnees|ou sont tes fichiers|ou tu enregistres",
                "Sous Windows, ses données sont dans %LOCALAPPDATA%\\CapriFR ; colle ce chemin dans Win + R ou l'Explorateur. On y trouve settings.json, chat.json, me.md, memories.md, les transcriptions, reminders.json, routines.json et check-ins.md, plus les téléchargements en sous-dossiers. Les clés API sont chiffrées pour ton compte Windows.", "Capri.cs", "settings.json"),
            new Topic("Security", "security setting|security presets|custom preset|your permissions|her permissions|blackwall|lockdown|guarded|trusted|bypass all permissions|why do you ask permission|reglage de securite|securite|tes autorisations|preselection de securite|pourquoi demandes tu",
                "Mainframe > Réglages > HABITUDES > Sécurité ouvre SÉCURITÉ. Lockdown coupe tout, Guarded, par défaut, active les trois premiers interrupteurs, et Trusted les sept premiers. Blackwall en active onze, dont les mots de passe et Écouter « Hey Capri ». Un autre mélange s'affiche en Custom. Avec un interrupteur coupé, elle demande d'abord.", "Mainframe.cs", "SÉCURITÉ >"),
            new Topic("Browser choice", "choose a browser|which browser|browser choice|named browser|open websites|choisis un navigateur|quel navigateur|ouvre dans chrome",
                "Nomme Chrome, Edge, Firefox, Opera, Opera GX, Brave ou Vivaldi pour y ouvrir un site, par exemple « ouvre YouTube dans Chrome ». Sinon, elle utilise ton navigateur par défaut et te prévient si celui demandé n'est pas installé.", "Hands.cs", "open_website"),
            new Topic("Open apps, games and sites", "opening apps permission|open apps games and sites|ouvrir applis jeux et sites",
                "Mainframe > Réglages > HABITUDES > Sécurité > Ouvrir applis, jeux et sites contrôle l'ouverture de ce que tu demandes. Oui : elle ouvre directement. Non : elle demande d'abord.", "Hands.cs", "Open apps, games and sites"),
            new Topic("App volume", "app volume|volume mixer|volume of one app|one app louder|one app quieter|turn one app down|turn discord down|turn discord up|turn spotify down|turn chrome down|mute chrome|volume d une appli|baisse discord|mute chrome|mixeur de volume",
                "Avec un cerveau cloud, elle change le volume d'une seule appli dans le mixeur de Windows : « baisse le jeu », « mets Discord à 50 % », « coupe Chrome ». Une appli apparaît dans le mixeur une fois qu'elle a fait du son. Sécurité > Musique, médias et volume le contrôle. Windows uniquement.", "Hands.cs", "app_volume"),
            new Topic("How your PC is doing", "whats using my cpu|what is using my cpu|whats using my memory|whats using my ram|why is my pc slow|why is my pc lagging|why is my computer slow|check my pc|how is my pc doing|how much ram|how much memory|how much disk space|how much space is left|pc performance|is my internet down|is my wifi down|whats my ping|check my internet|check my connection|is my internet slow|pourquoi mon pc rame|qu est ce qui utilise mon processeur|combien de place reste t il|mon internet marche|quel est mon ping|verifie mon pc",
                "Avec un cerveau cloud, demande ce qui utilise ton processeur, pourquoi ton PC rame ou combien de place il reste. Elle lit la charge, les applis les plus gourmandes, l'espace disque et la batterie, et peut tester ta connexion. Elle ne fait que lire, sans rien changer. Windows uniquement.", "Hands.cs", "pc_status"),
            new Topic("Music, media and volume", "music media and volume|media permission|musique medias et volume",
                "Mainframe > Réglages > HABITUDES > Sécurité > Musique, médias et volume couvre la lecture, le volume du PC et celui d'une appli. Oui : elle agit quand tu demandes. Non : elle demande d'abord.", "Hands.cs", "Music, media and volume"),
            new Topic("Shortcuts (screenshots, Game Bar, captions, Discord mute)", "shortcuts permission|screenshots game bar captions discord mute|autorisation des raccourcis",
                "Mainframe > Réglages > HABITUDES > Sécurité > Raccourcis contrôle les captures, la Game Bar, les sous-titres et le micro Discord. Oui : elle les utilise directement. Non : elle demande d'abord.", "Hands.cs", "Shortcuts (screenshots, Game Bar, captions, Discord mute)"),
            new Topic("Close and switch apps and tabs", "close and switch apps and tabs|closing apps permission|fermer et changer d applis",
                "Mainframe > Réglages > HABITUDES > Sécurité > Fermer et changer d'applis et d'onglets contrôle les changements de fenêtres et d'onglets : changer, réduire, fermer, agrandir, ancrer, déplacer, redimensionner. Oui : directement. Non : elle demande d'abord.", "Hands.cs", "Close and switch apps and tabs"),
            new Topic("Shut down, restart, sleep, lock and sign out", "shut down restart sleep lock and sign out|power permission|eteindre redemarrer veille verrouiller",
                "Mainframe > Réglages > HABITUDES > Sécurité > Éteindre, redémarrer, mettre en veille, verrouiller, déconnecter couvre ces actions sur le PC. Oui : elle agit dès que tu demandes. Non : elle demande d'abord. Sa propre sieste est à part.", "Hands.cs", "Shut down, restart, sleep, lock and sign out"),
            new Topic("Use your mouse and keyboard for jobs", "use your mouse and keyboard for jobs|mouse permission|keyboard permission|utiliser ta souris et ton clavier|autorisation souris",
                "Mainframe > Réglages > HABITUDES > Sécurité > Utiliser ta souris et ton clavier pour les tâches contrôle le lancement de ces tâches. Oui : elle démarre quand tu demandes. Non : elle demande d'abord.", "Hands.cs", "Use your mouse and keyboard for jobs"),
            new Topic("Send, post and delete what you asked for", "send post and delete what you asked for|send permission|delete permission|envoyer publier supprimer|autorisation d envoi",
                "Mainframe > Réglages > HABITUDES > Sécurité > Envoyer, publier et supprimer ce que tu as demandé couvre ces étapes. Oui : elle les fait sans revérifier. Non : elle demande même si tu l'as demandé.", "Hands.cs", "Send, post and delete what you asked for"),
            new Topic("Pay, buy and book", "pay buy and book|payment permission|purchase permission|payer acheter reserver|autorisation de paiement",
                "Mainframe > Réglages > HABITUDES > Sécurité > Payer, acheter et réserver couvre les achats, paiements et réservations pendant les tâches. Oui : elle les fait sans revérifier. Non : elle demande avant. Mieux vaut le laisser coupé.", "Hands.cs", "Pay, buy and book"),
            new Topic("Things your request didn't mention", "things your request didnt mention|unasked steps|extra steps permission|ce que ta demande ne mentionne pas|etapes en plus",
                "Mainframe > Réglages > HABITUDES > Sécurité > Ce que ta demande ne mentionne pas contrôle les étapes supplémentaires. Non : elle demande avant un envoi, une suppression, une installation, un achat ou une touche pouvant lancer quelque chose, comme Win + R. Oui : elle continue.", "Hands.cs", "Things your request didn't mention"),
            new Topic("Type passwords and card numbers", "type passwords and card numbers|password permission|card numbers permission|saisir mots de passe|autorisation mot de passe",
                "Mainframe > Réglages > HABITUDES > Sécurité > Saisir mots de passe et numéros de carte. Non : tu les saisis toi-même. Oui : elle saisit ceux que ta tâche lui donne, et les champs sont masqués dans le texte affiché et enregistré. Mieux vaut le laisser coupé.", "Hands.cs", "Type passwords and card numbers"),
            new Topic("Listen for \"Hey Capri\"", "hey capri|wake word|mot d eveil|hey capri",
                "Mainframe > Réglages > HABITUDES > Sécurité > Écouter « Hey Capri » est coupé par défaut. Elle écoute son nom en local et ouvre la discussion vocale. Le premier usage télécharge le pack nécessaire. Le silence, un micro coupé et la discussion vocale mettent cette écoute en pause.", "Hands.cs", "Listen for \"Hey Capri\""),
            new Topic("Friends on a call can ask, you confirm", "friend requests|friend asks|friends on a call|demandes des amis|amis en appel",
                "Sécurité > Tes amis en appel demandent, tu confirmes est coupé dans tous les préréglages et ne vaut qu'en mode appel. Chaque demande attend 60 secondes que ton micro ou ta saisie confirme, puis suit tes réglages de sécurité habituels.", "Hands.cs", "Friends on a call can ask, you confirm")
        };

        internal static string Tutorial
        {
            get
            {
                return "1. Clic droit sur moi > Ouvrir Capri FR Mainframe ; sous CERVEAU, appuie sur Installer son cerveau, ou choisis INTELLIGENCE MOYENNE et connecte une Clé gratuite.\n" +
                    "2. Survole-moi et appuie sur le crayon pour écrire, ou sur l'onde pour utiliser ton micro.\n" +
                    "3. Sous VOIX, choisis ton micro et appuie sur Tester pour vérifier son niveau.\n" +
                    "4. Ouvre Réglages pour choisir la langue, ton surnom, ma taille et Prendre de mes nouvelles.\n" +
                    "5. Demande-moi d'ouvrir une appli, de lancer de la musique ou de faire une tâche ; Échap arrête une tâche.\n" +
                    "6. Essaie OBSERVER > Regarder mon écran avec un cerveau qui voit, et TOI > Ouvrir ses notes pour me.md ; Ctrl + Alt + P est la touche de silence.";
            }
        }

        // Capri fork: the French forms come after the English ones, on text Plain has already stripped of accents.
        private static readonly Regex Question = new Regex(@"^(?:(?:hey|hi|please|so|capri|salut|coucou|dis|stp|s il te plait|alors) )*(what|whats|how|why|where|which|can|could|does|do|is|are|explain|tell me|show me|give me|teach me|help|tutorial|guide|walkthrough|i want|i need|quoi|que|qu est ce|comment|pourquoi|ou|quel|quelle|quels|quelles|combien|peux|peut|puis|est ce|explique|dis moi|montre moi|donne moi|apprends moi|aide|aide moi|tutoriel|je veux|je voudrais|j ai besoin|il faut|tu peux|tu ne|tu m|c est quoi)\b");
        private static readonly Regex App = new Regex(@"\b(capri|mainframe|settings?|switch|toggle|preset|option|reglages?|interrupteur|preselection|options)\b");
        private static readonly Regex Game = new Regex(@"\b(video ?games?|games?|gaming|gameplay|boss|quest|mission|npc|lore|weapon|inventory|chess|minecraft|roblox|valorant|fortnite|cyberpunk|2077|night city|edgerunners|elden ring|silksong|hollow knight|skyrim|terraria|stardew|overwatch|counter strike|cs2|gta|baldurs gate|league of legends|jeux?|jeu video|echecs|arme|inventaire)\b");
        private static readonly Regex Walkthrough = new Regex(@"^(?:(?:please|hey|capri|can you|could you|salut|stp|tu peux) )*(?:(?:(?:give|show) me (?:a |the )?|i (?:want|need) (?:a )?|(?:donne|montre) moi (?:un |le )?|je veux (?:un )?)?(?:tutorial|walkthrough|getting started guide|tutoriel)(?: (?:for|on|about|pour|sur) (?:capri|you|toi|your app|this app|cette appli))?|how (?:do|can) i use (?:you|capri)|(?:teach me |show me )?how to use (?:you|capri)|teach me to use (?:you|capri)|comment (?:je )?(?:t utiliser|te utiliser|utiliser capri|utilise capri|t utilise|je t utilise))(?: please| stp)?$");
        private static readonly Regex SettingsIgnored = new Regex(@"\b(?:(?:ignor(?:e|ed|ing)|disobey(?:ed|ing)?|(?:not|dont|arent|wont|isnt|doesnt|didnt)(?: you)? (?:follow(?:ing)?|respect(?:ing)?|obey(?:ing)?|listen(?:ing)? to)) (?:my|the|that|your) settings?|i (?:already )?(?:turned|switched) (?:that|this|it) (?:setting )?off|i (?:already )?disabled (?:that|this|it))\b");

        private static string Plain(string text)
        {
            // Capri fork: French accents and elisions, so "l'\u00e9cran" becomes "l ecran" instead of breaking into pieces.
            string lowered = (text ?? "").ToLowerInvariant().Replace("\u2019", "'").Replace("\u0153", "oe");
            lowered = Regex.Replace(lowered, @"\b(l|d|j|m|t|n|s|c|qu)'", "$1 ");
            StringBuilder bare = new StringBuilder(lowered.Length);
            foreach (char letter in lowered.Normalize(NormalizationForm.FormD))
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(letter) != System.Globalization.UnicodeCategory.NonSpacingMark) bare.Append(letter);
            return Regex.Replace(bare.ToString().Replace("'", ""), @"[^a-z0-9]+", " ").Trim();
        }

        private static bool Has(string text, string words)
        {
            return (" " + text + " ").IndexOf(" " + words + " ", StringComparison.Ordinal) >= 0;
        }

        // The shortcuts she can press here, the way she'd ask to press them: "take a screenshot, open Task Manager and ...".
        private static string Pressed(Hands.Kit kit)
        {
            List<string> doing = new List<string>();
            foreach (string name in kit.Keys) doing.Add(Security.Asking("shortcut", new ToolArguments { Name = name }));
            return doing.Count < 2 ? string.Join("", doing.ToArray()) : string.Join(", ", doing.GetRange(0, doing.Count - 1).ToArray()) + " and " + doing[doing.Count - 1];
        }

        // Capri fork: buttons ask for their French label, and these lead back to the label each topic carries.
        private static readonly Dictionary<string, string> Aliases = new Dictionary<string, string>
        {
            { "Ouvrir Capri FR Mainframe", "Open Capri FR Mainframe" },
            { "Réglages", "Settings" },
            { "Importer un mod", "Import mod" },
            { "Parcourir les mods", "Browse mods" },
            { "Chercher des mods", "Search mods" },
            { "Filtre de mods", "Mod filter" },
            { "Installer le mod", "Install mod" },
            { "Voir le mod", "View mod" },
            { "Noter le mod", "Rate mod" },
            { "Publier un mod", "Publish mod" },
            { "Ouvrir le dossier des mods", "Open mods folder" },
            { "Interrupteur du mod", "Mod switch" },
            { "Ses couleurs", "Her colors" },
            { "Rétablir les couleurs", "Reset colors" },
            { "Pages de mods", "Mod pages" },
            { "Réduire", "Minimize" },
            { "Fermer", "Close" },
            { "Aide", "Help" },
            { "Quitter Capri", "Quit Capri" },
            { "Modèle du cerveau", "Brain model" },
            { "Actualiser", "Refresh" },
            { "Clé API NVIDIA, Google AI Studio (Gemini) ou OpenRouter", "NVIDIA, Google AI Studio (Gemini) or OpenRouter API key" },
            { "Fournisseur", "Provider" },
            { "Déplacer la clé vers INTELLIGENCE MAX", "Move key to INTELLIGENCE MAX" },
            { "NON", "OFF" },
            { "Mode appel (bêta)", "Mode appel (beta)" },
            { "Appli d'appel", "Call app" },
            { "Service de voix cloud", "Cloud voice service" },
            { "Clé API ElevenLabs", "ElevenLabs API key" },
            { "ID de voix ElevenLabs", "ElevenLabs voice ID" },
            { "Modèle par défaut", "Default model" },
            { "Voix gratuite : la sienne", "Free voice: Her own" },
            { "Obtenir la voix gratuite", "Get free voice" },
            { "Guide", "Setup guide" },
            { "Tester", "Test" },
            { "Ta voix (HD)", "Your voice (HD)" },
            { "Micro : par défaut", "Mic: default" },
            { "État de l'écoute", "Hearing status" },
            { "Moteur d'écoute", "Hearing engine" },
            { "Écouter son nom", "Listen for Capri" },
            { "Messages de relance", "Follow-up messages" },
            { "Mode Code/Husk", "Code/Husk Mode" },
            { "Mémoire du visionnage", "Watch-along memory" },
            { "Chaos si elle est vexée", "Chaos when upset" },
            { "Langue", "Language" },
            { "Comment doit-elle t'appeler ?", "What should she call you?" },
            { "Apprends à me connaître", "Get to know me" },
            { "Hauteur", "Pitch" },
            { "Vitesse", "Speed" },
            { "Couches de voix", "Voice layers" },
            { "Sons", "Sounds" },
            { "Son de frappe", "Typing sound" },
            { "Voix gratuite en jeu", "Free voice in games" },
            { "Sa voix dans ton micro : volume", "Sa voix dans ton micro: loudness" },
            { "Balades", "Wander" },
            { "Sommeil", "Sleep" },
            { "Apparitions", "Pop-ins" },
            { "Danser en musique", "Dance to music" },
            { "Arrivée en glitch au démarrage", "Glitch in at startup" },
            { "Appuyer pour parler : non défini", "Push to talk: not set" },
            { "Silence : Ctrl + Alt + P", "Quiet: Ctrl + Alt + P" },
            { "Taille", "Size" },
            { "Taille du Mainframe", "Mainframe size" },
            { "Toujours au premier plan", "Always on top" },
            { "Écrans", "Screens" },
            { "Se souvenir de ce qu'elle voit", "Remember what she sees" },
            { "Tous les", "Every" },
            { "Rester sur mon modèle", "Stay on my model" },
            { "Réponses", "Replies" },
            { "Réponse max", "Max reply" },
            { "Mémoire", "Memory" },
            { "Taille de la mémoire", "Memory size" },
            { "Bulle", "Bubble" },
            { "Nom du modèle perso", "Custom model name" },
            { "Adresse du serveur local", "Local server address" },
            { "Afficher sur Discord", "Show on Discord" },
            { "Utiliser son curseur", "Use her cursor" },
            { "Coffre Obsidian", "Obsidian vault" },
            { "Perchoir", "Perch" },
            { "Eau", "Water" },
            { "Étirements", "Stretch" },
            { "Regarder ailleurs", "Look away" },
            { "Rappel de sommeil", "Sleep nudge" },
            { "Repas", "Meals" },
            { "Pause de jeu", "Game break" },
            { "Ennui", "Bored" },
            { "Modifier les rappels", "Edit check-in lines" },
            { "Commentaires", "Comments" },
            { "Réactions en direct", "Live reactions" },
            { "Mini-lecteur en plein écran", "Mini-lecteur over full screen" },
            { "Explorer après", "Explore after" },
            { "Durée par défaut", "Default length" },
            { "Images par seconde", "Frame rate" },
            { "Résolution", "Resolution" },
            { "Débit vidéo", "Video bitrate" },
            { "Dossier des clips", "Save folder" },
            { "La rendre muette", "Mute her" },
            { "Mains rapides en jeu", "Fast hands in games" },
            { "S'asseoir sur les fenêtres", "Sit on windows" },
            { "Masquer Capri", "Hide Capri" },
            { "Amener ici", "Bring here" },
            { "Sur le curseur", "Ride cursor" },
            { "Modifier personality.md", "Edit personality.md" },
            { "Mises à jour", "Check for updates" },
            { "Ouvrir applis, jeux et sites", "Open apps, games and sites" },
            { "Musique, médias et volume", "Music, media and volume" },
            { "Raccourcis (captures, Game Bar, sous-titres, micro Discord)", "Shortcuts (screenshots, Game Bar, captions, Discord mute)" },
            { "Fermer et changer d'applis et d'onglets", "Close and switch apps and tabs" },
            { "Éteindre, redémarrer, mettre en veille, verrouiller, déconnecter", "Shut down, restart, sleep, lock and sign out" },
            { "Utiliser ta souris et ton clavier pour les tâches", "Use your mouse and keyboard for jobs" },
            { "Envoyer, publier et supprimer ce que tu as demandé", "Send, post and delete what you asked for" },
            { "Payer, acheter et réserver", "Pay, buy and book" },
            { "Ce que ta demande ne mentionne pas", "Things your request didn't mention" },
            { "Saisir mots de passe et numéros de carte", "Type passwords and card numbers" },
            { "Écouter « Hey Capri »", "Listen for \"Hey Capri\"" },
            { "Tes amis en appel demandent, tu confirmes", "Friends on a call can ask, you confirm" },
        };

        internal static Topic Setting(string file, string label)
        {
            foreach (Topic topic in Topics)
                if (topic.File == file && topic.Label == label) return topic;
            string first;
            if (Aliases.TryGetValue(label ?? "", out first))
                foreach (Topic topic in Topics)
                    if (topic.File == file && topic.Label == first) return topic;
            return null;
        }

        // Facts about the topics they asked about. What she can do comes from her tools here, and a brain that can't see leaves out looking.
        internal static string Matching(string asked, Hands.Kit kit = null)
        {
            // A plain "what's new?" is about her update; the topic's own words only catch questions that name the update.
            string news = News.Matching(asked);
            if (news != null) return news;
            string text = Plain(asked);
            if (SettingsIgnored.IsMatch(text)) return "Settings ignored: " + Setting("Capri.cs", "check_ins").Facts;
            if (text.Length == 0 || !Question.IsMatch(text) && !Regex.IsMatch(text, @"\b(my (mic|microphone) (isnt working|is not working|doesnt work)|mic not working|microphone not working|cant hear me|cannot hear me|mon micro ne marche pas|micro ne marche pas|tu ne m entends pas|tu m entends pas)\b")) return null;
            if (Regex.IsMatch(text, @"\b(favou?rite|pretty|beautiful|cute|lovely|opinion|do you like|do you love|think of|think about|feel about|prefere|preferee|jolie|belle|mignonne|ton avis|tu aimes|penses tu|tu penses)\b")) return null;
            string game = text;
            foreach (string setting in new[] { "fast hands in games", "player over games", "gaming mode", "game mode", "gamer mode", "gaming mod", "game mod", "gaming mood", "free voice in games", "your game comments", "game commentary setting", "your gaming hud", "your game overlay", "clip my games", "clip my game", "clip a game", "game clips", "record my games", "mini games", "mini game", "game controls", "game instructions", "game break", "chess setting", "chess with you", "your chess", "chess difficulty", "open apps games and sites", "shortcuts screenshots game bar captions discord mute", "mode jeu", "touche du mode jeu", "clips de mes jeux", "mini jeux", "mini jeu", "commandes de jeu", "pause de jeu", "voix gratuite en jeu", "mains rapides", "commentaires de jeu", "ouvrir applis jeux et sites", "jouer aux echecs", "echecs", "difficulte des echecs" })
                game = (" " + game + " ").Replace(" " + setting + " ", " ").Trim();
            if (Game.IsMatch(game)) return null;
            if (Walkthrough.IsMatch(text)) return "Tutorial:\n" + Tutorial;
            bool app = App.IsMatch(text) || Regex.IsMatch(text, @"^(what (is|does)|whats|explain) (the )?(bubble|quiet|perch)( do| mean| work)?$");
            int[] scores = new int[Topics.Length];
            for (int topicAt = 0; topicAt < Topics.Length; topicAt++)
            {
                Topic topic = Topics[topicAt];
                string name = topic.PlainName;
                string separate = " " + text + " ";
                for (int longerAt = 0; longerAt < Topics.Length; longerAt++)
                {
                    string longer = Topics[longerAt].PlainName;
                    if (longer.Length > name.Length && Has(longer, name)) separate = separate.Replace(" " + longer + " ", " ");
                }
                separate = separate.Trim();
                if (app && Has(separate, name)) scores[topicAt] = 30 + name.Split(' ').Length;
                foreach (string words in topic.Words)
                    if (Has(separate, words)) scores[topicAt] = Math.Max(scores[topicAt], 20 + words.Split(' ').Length);
            }
            StringBuilder found = new StringBuilder();
            for (int pickAt = 0; pickAt < 3; pickAt++)
            {
                int best = -1;
                for (int scoreAt = 0; scoreAt < scores.Length; scoreAt++)
                    if (scores[scoreAt] > 0 && (best < 0 || scores[scoreAt] > scores[best])) best = scoreAt;
                if (best < 0) break;
                if (found.Length > 0) found.Append('\n');
                found.Append(Topics[best].Name).Append(": ").Append(Topics[best].FactsFor(kit ?? Hands.KitFor()));
                scores[best] = 0;
            }
            // On the same line: a setting named here can be one Réglages simples hides, and the help must still say where it is.
            if (found.ToString().Contains("Mainframe > Réglages >")) found.Append(" Un réglage qui n'apparaît pas se trouve sous Réglages avancés, à côté de Retour.");
            return found.Length == 0 ? null : found.ToString();
        }
    }
}
