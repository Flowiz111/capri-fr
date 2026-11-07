# Capri FR

> **Version française non officielle de [Capri](https://github.com/lachydotmcg/capri-desktop).** Ce fork n'est ni géré ni approuvé par le projet Capri. Pour la version d'origine, en anglais, va sur [son dépôt](https://github.com/lachydotmcg/capri-desktop).

Capri FR est une compagne de bureau animée pour Windows, avec une interface en français. Elle vit sur ton écran, se déplace toute seule, discute avec toi à l'écrit ou à la voix et, une fois reliée à une IA, peut t'aider sur ton PC.

**[⬇️ Télécharger la dernière version](../../releases/latest)** · **[📖 Lire l'aide complète](AIDE.md)** · **[🐞 Signaler un problème](../../issues)**

Required Notice: Copyright 2026 Lachy (https://github.com/lachydotmcg)

Required Notice: Anyone who uses Capri's code, art, animations or any of its other assets must credit the Capri project (https://github.com/lachydotmcg/capri-desktop). Capri's art and animations are by her artist, who prefers to stay private. Parts made by others keep their own licences, listed in its README.

## Sommaire

- [Ce qu'elle sait faire](#ce-quelle-sait-faire)
- [Configuration requise](#configuration-requise)
- [Installer](#installer)
- [Premiers pas](#premiers-pas)
- [Choisir son cerveau](#choisir-son-cerveau)
- [Raccourcis](#raccourcis)
- [Mods](#mods)
- [Sécurité et vie privée](#sécurité-et-vie-privée)
- [Mettre à jour et désinstaller](#mettre-à-jour-et-désinstaller)
- [Ce qui change par rapport à Capri](#ce-qui-change-par-rapport-à-capri)
- [Limites connues](#limites-connues)
- [Compiler depuis le code](#compiler-depuis-le-code)
- [Licence et crédits](#licence-et-crédits)

## Ce qu'elle sait faire

**Sur ton bureau**

- Elle se balade, s'assoit sur tes fenêtres, s'allonge, dort et danse quand elle entend de la musique.
- Elle se téléporte dans un glitch, saute d'une fenêtre à l'autre, fait coucou depuis le bord de l'écran et attrape ton curseur.
- Elle te propose parfois un tour, une partie, de la musique ou un rappel de pause, et n'agit qu'après ton oui.
- Tu peux la faire glisser, la lancer ou lui caresser la tête.

**Pour discuter**

- À l'écrit ou à la voix, en français, avec une écoute qui fonctionne hors ligne.
- Elle retient ce que tu lui demandes et répond à tes questions sur ses propres réglages.
- Elle parle avec les voix de Windows, ton propre modèle ou un service de voix en ligne.

**Pour t'aider, une fois reliée à une IA**

- Ouvrir des applications et des sites, lancer de la musique, régler le volume, gérer tes fenêtres.
- Regarder ton écran si tu l'actives, et réagir à tes jeux ou à tes vidéos.
- Faire des tâches avec ta souris et ton clavier, selon le niveau de sécurité que tu choisis.

**Pour s'amuser**

- Échecs, Puissance 4, morpion et pierre-feuille-ciseaux, même sans IA.
- Un mode jeu, des clones, un mode hologramme et des mods.

## Configuration requise

| | |
| --- | --- |
| **Système** | Windows 10 ou 11, 64 bits. Vérifié uniquement sous Windows 11. |
| **Espace disque** | Environ 1 Go pour l'application, sa voix et son écoute, plus 3 Go environ pour un cerveau local. |
| **Mémoire** | Un cerveau local demande un PC assez puissant : l'application indique ce qui convient au tien. Avec une clé en ligne, un PC modeste suffit. |
| **Internet** | Pour installer, télécharger les modèles et utiliser un cerveau en ligne. Un cerveau local fonctionne ensuite sans connexion. |

## Installer

1. Ouvre la [page des versions](../../releases/latest) et télécharge `CapriFR-Setup.exe`.
2. Lance le fichier. L'application n'est pas signée, donc Windows peut afficher « Windows a protégé votre ordinateur » : clique sur **Informations complémentaires**, puis **Exécuter quand même** si tu fais confiance à cette source.
3. Clique sur **Installer Capri FR**. Le modèle vocal se télécharge pendant l'installation (plusieurs centaines de Mo).

**Vérifier le fichier :** certains antivirus signalent ce type d'application, parce qu'elle prend des captures d'écran et peut utiliser la souris et le clavier. Une alerte seule ne prouve pas que le fichier est sûr. La page de la version donne son empreinte SHA-256 : compare-la avec le résultat de cette commande PowerShell.

```powershell
Get-FileHash .\CapriFR-Setup.exe -Algorithm SHA256
```

## Premiers pas

1. **Ouvre ses réglages :** fais un clic droit sur le personnage, puis choisis **Ouvrir Capri FR Mainframe**.
2. **Donne-lui un cerveau :** voir [ci-dessous](#choisir-son-cerveau). Sans cerveau, elle ne peut pas te répondre.
3. **Règle ton micro :** dans la section **VOIX**, choisis ton micro et appuie sur **Tester**.
4. **Parle-lui :** survole-la, puis clique sur le crayon pour écrire ou sur l'onde pour parler.

**Si elle ne répond pas à la voix :** avec **Écouter son nom** activé, seules les phrases contenant « Capri » lui parviennent, par exemple « Capri, quelle heure est-il ? ». Sinon, ouvre la discussion avec `Ctrl + Alt + V`. Les autres solutions sont dans [l'aide](AIDE.md#ça-ne-marche-pas-).

## Choisir son cerveau

| Niveau | Coût | Où vont tes échanges | Bon à savoir |
| --- | --- | --- | --- |
| **NORMAL** | Gratuit | Restent sur ton PC | Environ 3 Go à télécharger. Dépend de la puissance de ton PC. Fonctionne aussi avec Ollama. |
| **INTELLIGENCE MOYENNE** | Gratuit | Chez NVIDIA, Google ou OpenRouter | Il faut une clé gratuite. Parfois lent et limité. |
| **INTELLIGENCE MAX** | Selon le service | Chez le service choisi | Accepte aussi OpenAI, Claude ou DeepSeek, avec ta propre clé. |

Le bouton **Fournisseur** permet aussi de se connecter à un serveur compatible OpenAI, comme LM Studio. Les clés enregistrées sont chiffrées pour ton compte Windows.

## Raccourcis

| Touches | Action |
| --- | --- |
| `Ctrl + Alt + V` | Ouvrir ou fermer la discussion vocale |
| `Ctrl + Alt + N` | Couper ou rétablir le micro |
| `Ctrl + Alt + T` | Écrire |
| `Ctrl + Alt + H` | Masquer ou afficher |
| `Ctrl + Alt + P` | Silence : plus d'interventions spontanées |
| `Ctrl + Alt + G` | Mode jeu |
| `Échap` | Arrêter une tâche |

Tu peux les modifier dans Mainframe > Réglages > TOUCHES.

## Mods

Ouvre Mainframe > **Mods** pour parcourir [l'annuaire de mods](https://github.com/Flowiz111/capri-mods), installer un mod ou importer un fichier.

| Type | Extension | Ce qu'il fait |
| --- | --- | --- |
| **Réglages** | `.capri-mod` | Change des noms, des couleurs ou d'autres réglages. |
| **Apparence** | `.capri-skin` | Remplace ses dessins. |
| **Code** | `.capri-code` | Exécute du code avec un **accès complet à ton PC**. Elle demande avant de le lancer. |

N'installe que des mods en lesquels tu as confiance, surtout les mods de code : ceux de l'annuaire ont été copiés depuis l'annuaire d'origine de Capri, sans contrôle supplémentaire. Les mods appartiennent à leurs auteurs.

## Sécurité et vie privée

**Son niveau de sécurité** se règle dans Mainframe > Réglages > HABITUDES > Sécurité.

| Préréglage | Ce qu'elle fait sans demander |
| --- | --- |
| **Lockdown** | Rien. |
| **Guarded** (par défaut) | Applis, musique et raccourcis. |
| **Trusted** | En plus : fenêtres, extinction du PC, souris et clavier, envois que tu as demandés. |
| **Blackwall** | Tout, y compris mots de passe et cartes. **À éviter.** |

**Ce qu'il faut savoir :**

- **Regarder mon écran** est coupé par défaut. Une fois activé avec un cerveau en ligne, ce qui est affiché à l'écran part chez ce service, y compris tes messages.
- **Démarrage :** une copie installée se lance avec Windows. Cela se coupe dans Mainframe > Réglages > HABITUDES > Lancer avec Windows.
- **Statut Discord :** activé par défaut. Il se coupe dans Mainframe > Réglages > HABITUDES > Afficher sur Discord.
- **Tes données :** réglages, discussions et notes restent dans `%LOCALAPPDATA%\CapriFR`. Capri FR n'envoie aucune statistique.
- **Connexions :** l'application contacte GitHub pour vérifier les versions et télécharger ses modèles et ses mods. Certains modèles viennent des pages du projet Capri d'origine et de huggingface.co.

Le détail est dans [l'aide](AIDE.md#vie-privée) et [SECURITY.md](SECURITY.md).

## Mettre à jour et désinstaller

**Mettre à jour :** elle te prévient quand une nouvelle version sort, et le bouton **Mises à jour** du Mainframe vérifie tout de suite. Elle n'installe jamais rien toute seule. Quitte-la avec **Quitter Capri**, puis installe la nouvelle version par-dessus. Tes données sont conservées.

**Désinstaller :** passe par Paramètres Windows > Applications. Le programme, ses modèles, ses raccourcis et le lancement au démarrage sont retirés, mais **tes discussions, ta mémoire et tes réglages restent** dans `%LOCALAPPDATA%\CapriFR`. Pour tout effacer, supprime ce dossier ensuite.

## Ce qui change par rapport à Capri

Capri FR repose sur **Capri v0.8.3**, et son code se trouve sur la branche `fr` de ce dépôt.

- Interface, réponses d'aide, messages et mini-jeux en français, avec le français par défaut.
- Voix française par les voix de Windows, voix de femme en priorité.
- Sécurité **Guarded** par défaut, au lieu de Trusted.
- Quatre déplacements en plus et des propositions exécutées seulement après ton oui.
- Rappel dans sa bulle quand une phrase est ignorée faute d'avoir dit son nom.
- Vérification des versions sur ce dépôt, sans installation automatique.
- Page d'aide en français. Boutons de soutien et de Discord retirés.
- Annuaire de mods sur [Flowiz111/capri-mods](https://github.com/Flowiz111/capri-mods), avec des descriptions en français, sans notes ni compteurs de téléchargements.
- Nom, fichiers et dossier de données distincts (`CapriFR.exe`, `%LOCALAPPDATA%\CapriFR`), pour cohabiter avec une vraie Capri.

## Limites connues

- **Correctifs de Capri :** les corrections publiées par le projet d'origine après la v0.8.3 ne sont pas incluses.
- **Traduction :** il reste de l'anglais dans la fiche « What's new », les fiches réservées au modèle, des messages techniques rares, les noms de couleurs et certains titres des mods, et les préréglages de sécurité. Les instructions destinées au modèle d'IA restent en anglais, et il répond en français. Certaines fiches d'aide sont condensées.
- **Fenêtre Mainframe :** elle peut dépasser de l'écran à l'ouverture.
- **Mods :** seuls ceux compatibles avec la v0.8.3 fonctionnent, et tous n'ont pas été testés.
- **Tests :** la suite complète de Capri (`--smoke-test`) n'a pas été adaptée à cette version. Vérifications faites sous Windows 11 uniquement. Les versions Mac et Linux et l'application Quest de ce dépôt ne sont ni traduites ni testées.
- **Installateur non signé :** Windows et certains antivirus peuvent afficher des alertes.

## Compiler depuis le code

```powershell
powershell -ExecutionPolicy Bypass -File ci\build-windows.ps1
```

Il faut Windows, le .NET Framework 4.8 et le SDK Windows. Le résultat se trouve dans `out\windows`.

**Avant de publier une version :** mets `FrRevision` dans `app/Capri.cs` au même numéro que l'étiquette de la version (`v0.8.3-fr.N`), sinon l'alerte de mise à jour se trompe.

| Dossier | Contenu |
| --- | --- |
| `app/` | Le code de l'application Windows |
| `assets/` | Les dessins, polices et sons |
| `ci/` | Les scripts de compilation |
| `mods/` | Des exemples de mods |
| `cross/`, `quest/`, `site/`, `worker/` | Versions et outils du projet d'origine, non adaptés à Capri FR |

## Licence et crédits

- **Code, dessins et animations :** projet [Capri](https://github.com/lachydotmcg/capri-desktop) de Lachy, avec le travail de « Capri's artist », sous licence [PolyForm Noncommercial 1.0.0](LICENSE.md).
- **Usage non commercial uniquement :** pas de vente, de version payante ni d'usage commercial. En cas de partage, garde ce crédit, la licence et les lignes `Required Notice`.
- **Composants tiers :** voir [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) et la liste du [README d'origine](README-Capri-original.md).
- **Création de fans :** Capri est un projet de fans non officiel, qui n'est ni approuvé ni soutenu par CD PROJEKT RED, les ayants droit de l'anime ou ses comédiens de doublage. La licence ne donne aucun droit sur ce qui leur appartient.
