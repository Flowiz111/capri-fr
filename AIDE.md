# Aide de Capri FR

Capri FR est la version française non officielle de [Capri](https://github.com/lachydotmcg/capri-desktop). Cette page concerne Capri FR : le site de Capri décrit le logiciel d'origine, qui diffère sur certains points.

**Sommaire :** [Démarrer](#démarrer) · [Lui parler](#lui-parler) · [Ça ne marche pas ?](#ça-ne-marche-pas-) · [Son cerveau](#son-cerveau) · [Sa voix](#sa-voix) · [Raccourcis](#raccourcis) · [Sécurité](#sécurité) · [Vie privée](#vie-privée) · [Mettre à jour](#mettre-à-jour) · [Désinstaller](#désinstaller) · [Signaler un problème](#signaler-un-problème)

## Démarrer

1. Installe `CapriFR-Setup.exe` depuis la [page des versions](../../releases/latest). Si Windows affiche « Windows a protégé votre ordinateur », clique sur **Informations complémentaires**, puis **Exécuter quand même** si tu fais confiance à cette source.
2. Fais un clic droit sur le personnage, puis choisis **Ouvrir Capri FR Mainframe**. C'est la fenêtre de tous ses réglages.
3. Donne-lui un cerveau : voir [Son cerveau](#son-cerveau). Sans cerveau, elle ne peut pas te répondre.
4. Dans la section **VOIX**, choisis ton micro et appuie sur **Tester** pour vérifier son niveau.
5. Survole le personnage : le crayon sert à écrire, l'onde à parler.

Les réglages plus détaillés se trouvent dans Mainframe > **Réglages**. Tu peux aussi lui poser tes questions directement, par exemple « comment changer ta voix ? ».

## Lui parler

**Pour écrire :** survole-la et clique sur le crayon, ou appuie sur `Ctrl + Alt + T`.

**À la voix, il y a deux façons de faire :**

| Mode | Comment ça marche |
| --- | --- |
| **Discussion vocale** | `Ctrl + Alt + V` ou l'onde ouvre et ferme la discussion. Dans ce mode, tes phrases lui parviennent sans que tu dises son nom. |
| **Écouter son nom** | Active-le dans Mainframe > Réglages > VOIX. Le micro reste ouvert, et **seules les phrases contenant « Capri » lui parviennent**. Exemple : « Capri, quelle heure est-il ? ». |

« Écouter son nom » est coupé par défaut. Une fois activé, `Ctrl + Alt + V` ferme ou rouvre ses oreilles.

Son écoute fonctionne hors ligne et gère le français. Tu peux aussi lui attribuer une touche dans Réglages > TOUCHES > Appuyer pour parler.

## Ça ne marche pas ?

| Problème | À essayer |
| --- | --- |
| Elle ne répond pas quand je parle | Si « Écouter son nom » est activé, commence ta phrase par « Capri ». Sinon, ouvre la discussion avec `Ctrl + Alt + V`. Vérifie que le micro n'est pas coupé (`Ctrl + Alt + N`) et teste-le dans Mainframe > VOIX. |
| Le micro n'entend rien | Choisis la bonne entrée dans Mainframe > VOIX > Micro, puis appuie sur **Tester**. Vérifie aussi Paramètres Windows > Confidentialité et sécurité > Microphone. |
| Elle dit qu'il faut installer sa puce | Elle n'a pas encore de cerveau : voir [Son cerveau](#son-cerveau). |
| Elle répond lentement | Les clés gratuites peuvent être lentes et limitées, surtout NVIDIA aux heures de pointe. Essaie un autre modèle ou un cerveau local. |
| Elle ne voit pas mon écran | Il faut un cerveau qui voit, et la plupart des modèles gratuits de NVIDIA ne voient pas. |
| Sa voix n'est pas bonne | En français, elle parle avec une voix de Windows. Tu peux en ajouter dans Paramètres Windows > Heure et langue > Voix. |
| La fenêtre Mainframe dépasse de l'écran | Elle peut être plus haute que certains écrans. Essaie de changer Réglages > CAPRI > Taille du Mainframe. C'est un défaut connu, qui n'est pas encore corrigé. |
| Je ne la vois plus | Double-clique sur son icône près de l'horloge pour la ramener. Dans le Mainframe, **Amener ici** et **Afficher Capri** servent aussi. |
| Elle fait une tâche que je veux arrêter | Appuie sur `Échap`. Bouger la souris ne l'arrête pas. |
| Des clones envahissent mon écran | Maintiens `Échap`, ou dis « débarrasse-toi de tes clones ». |
| Windows ou mon antivirus l'a bloquée | L'application n'est pas signée et elle prend des captures et utilise la souris, ce qui ressemble au comportement de certains logiciels malveillants. Une alerte seule ne prouve pas que le fichier est sûr : compare son empreinte avec celle de la page de la version. |

**Vérifier l'empreinte dans PowerShell, depuis ton dossier de téléchargement :**

```powershell
Get-FileHash .\CapriFR-Setup.exe -Algorithm SHA256
```

Le résultat doit être identique à l'empreinte SHA-256 indiquée sur la [page de la version](../../releases/latest). Garde ton antivirus activé.

## Son cerveau

Choisis un niveau dans Mainframe > **CERVEAU**. Un seul tourne à la fois.

| Niveau | Coût | Ce qu'il faut savoir |
| --- | --- | --- |
| **NORMAL** | Gratuit | Modèle local sur ton PC : appuie sur **Installer son cerveau** (le téléchargement dépend du modèle, environ 3,1 Go pour celui proposé sur notre PC de test), ou utilise Ollama et appuie sur **Actualiser**. |
| **INTELLIGENCE MOYENNE** | Gratuit | Clé gratuite NVIDIA, Google AI Studio (Gemini) ou OpenRouter. Parfois lente et limitée. Tes échanges partent chez ce service. |
| **INTELLIGENCE MAX** | Selon le service | Accepte aussi OpenAI, Claude ou DeepSeek, avec ta propre clé. Tes échanges partent chez ce service. |

Le bouton **Fournisseur** permet aussi de se connecter à un serveur compatible OpenAI, comme LM Studio.

Les clés enregistrées sont chiffrées pour ton compte Windows. Ne les partage avec personne.

## Sa voix

Mainframe > **VOIX** propose :

- **NON** : texte seul.
- **LOCALE INTÉGRÉE** : voix gratuites sur ton PC. En français, c'est une voix de Windows, féminine en priorité.
- **LOCALE PERSO** : ton propre modèle.
- **VOIX CLOUD** : Fish Audio, ElevenLabs ou OpenAI, avec ta clé.

Ne clone jamais la voix d'une personne vivante sans son accord explicite.

## Raccourcis

| Touches | Action |
| --- | --- |
| `Ctrl + Alt + V` | Parler : ouvre ou ferme la discussion vocale |
| `Ctrl + Alt + N` | Couper ou rétablir le micro |
| `Ctrl + Alt + T` | Écrire |
| `Ctrl + Alt + H` | Masquer ou afficher |
| `Ctrl + Alt + P` | Silence : plus d'interventions spontanées, mais elle répond toujours |
| `Ctrl + Alt + G` | Mode jeu |
| `Échap` | Arrêter une tâche |

Tu peux les changer dans Mainframe > Réglages > TOUCHES.

**Clic sur le personnage :** fais-la glisser pour la déplacer, double-clique pour la mettre sur le curseur, fais un clic droit pour ouvrir son menu.

## Sécurité

Une fois reliée à une IA, elle peut ouvrir des applications et, selon tes réglages, utiliser ta souris et ton clavier. Le niveau se règle dans Mainframe > Réglages > HABITUDES > Sécurité.

| Préréglage | Ce qu'il autorise sans demander |
| --- | --- |
| **Lockdown** | Rien : elle demande avant tout. |
| **Guarded** (par défaut) | Applis, musique et raccourcis. Elle demande pour le reste. |
| **Trusted** | En plus : gérer les fenêtres, éteindre le PC, utiliser la souris et le clavier, envoyer ce que tu as demandé. Elle demande avant de payer ou pour ce que ta demande ne mentionne pas. |
| **Blackwall** | Tout, y compris saisir mots de passe et cartes, et elle écoute « Hey Capri ». **À éviter.** |

**Mods :** un fichier `.capri-mod` change seulement des réglages. Un fichier `.capri-code` exécute du code avec un **accès complet à ton PC** : n'installe que ce en quoi tu as confiance.

Pendant qu'elle travaille, garde un œil sur ce qu'elle fait. L'IA peut se tromper.

## Vie privée

Capri FR n'envoie aucune statistique ni aucun rapport au projet Capri ou à ce dépôt. Tes réglages, discussions, transcriptions et notes restent dans `%LOCALAPPDATA%\CapriFR`.

**Ce qui peut quitter ton PC :**

| Quoi | Où ça va | Quand |
| --- | --- | --- |
| Discussions, transcriptions, notes, souvenirs, captures d'écran, fichiers utiles à une tâche | Le service d'IA choisi | Si tu utilises un cerveau en ligne. Avec un cerveau local, tout reste sur ton PC. |
| Texte à prononcer | Le service de voix choisi | Si tu utilises une voix cloud |
| Tes recherches et demandes de musique ou de sites | DuckDuckGo, YouTube ou le site demandé | Quand tu le demandes |
| Son activité | L'application Discord sur ton PC | Activé par défaut. Se coupe dans Réglages > HABITUDES > Afficher sur Discord. |
| Téléchargement des modèles | GitHub, huggingface.co | Quand ils sont installés |

**⚠️ Regarder mon écran :** ce réglage est coupé par défaut. Une fois activé, elle prend des captures de ton écran de sa propre initiative. Avec un cerveau en ligne, **tout ce qui est affiché part chez ce service**, y compris tes messages, tes mails ou tes mots de passe visibles. Coupe-le dans Mainframe > Réglages > OBSERVER si tu travailles sur quelque chose de privé.

**Clés gratuites :** vérifie les conditions du service choisi, car ce qu'il conserve de tes données dépend de lui.

**Tout effacer :** voir [Désinstaller](#désinstaller).

## Mettre à jour

Capri FR ne se met pas à jour toute seule. Elle ne reçoit donc pas non plus automatiquement les correctifs de sécurité de Capri, et elle repose sur Capri v0.8.3.

Pour mettre à jour, installe la dernière version depuis la [page des versions](../../releases/latest) par-dessus l'ancienne. Quitte d'abord Capri FR avec **Quitter Capri** dans le Mainframe. Tes données sont conservées.

## Désinstaller

Passe par Paramètres Windows > Applications, ou lance le désinstallateur depuis le dossier de l'application.

- **Retirés :** le programme, le cerveau, la voix, le modèle d'écoute, les raccourcis et le lancement au démarrage.
- **Gardés :** tes discussions, ta mémoire et tes réglages, dans `%LOCALAPPDATA%\CapriFR`.

**Pour tout effacer :** colle `%LOCALAPPDATA%\CapriFR` dans la barre d'adresse de l'Explorateur, puis supprime ce dossier après la désinstallation.

**Pour qu'elle ne se lance plus au démarrage sans la désinstaller :** Mainframe > Réglages > HABITUDES > Lancer avec Windows.

## Signaler un problème

Ouvre une [issue sur ce dépôt](../../issues), pas auprès du projet Capri. Indique ta version de Windows, ce que tu faisais et ce qui s'est passé. **Ne publie jamais de clé API, de transcription ou de capture contenant des informations privées.**
