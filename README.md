# Capri FR

> **Version française non officielle de [Capri](https://github.com/lachydotmcg/capri-desktop).** Ce fork n'est ni géré ni approuvé par le projet Capri. Pour la version d'origine, en anglais, va sur [son dépôt](https://github.com/lachydotmcg/capri-desktop).

Capri FR est une compagne de bureau animée pour Windows, avec une interface en français. Elle se déplace sur ton écran, discute avec toi et, une fois reliée à une IA, peut t'aider sur ton PC.

Required Notice: Copyright 2026 Lachy (https://github.com/lachydotmcg)
Required Notice: Anyone who uses Capri's code, art, animations or any of its other assets must credit the Capri project (https://github.com/lachydotmcg/capri-desktop). Capri's art and animations are by her artist, who prefers to stay private. Parts made by others keep their own licences, listed in its README.

## Installer

1. Ouvre la [page des versions](../../releases/latest) et télécharge `CapriFR-Setup.exe`.
2. Lance le fichier. L'application n'est pas signée, donc Windows peut afficher « Windows a protégé votre ordinateur » : clique sur **Informations complémentaires**, puis **Exécuter quand même** si tu fais confiance à cette source.
3. Clique sur **Installer Capri FR**. Le modèle vocal se télécharge pendant l'installation (plusieurs centaines de Mo).
4. Fais un clic droit sur le personnage, puis ouvre **Mainframe**. Clique sur **Installer son cerveau** pour la faire répondre (environ 3,1 Go selon le modèle).

**Vérifier le fichier :** certains antivirus signalent ce type d'application, parce qu'elle prend des captures d'écran et peut utiliser la souris et le clavier. La page de la version donne l'empreinte SHA-256 de l'installateur. Compare-la avec le résultat de cette commande PowerShell :

```powershell
Get-FileHash .\CapriFR-Setup.exe -Algorithm SHA256
```

## Aide

Tout est expliqué dans **[l'aide de Capri FR](AIDE.md)** : démarrer, lui parler, résoudre les problèmes, sécurité et vie privée. Le bouton **Aide** de l'application ouvre cette page.

## À savoir avant de l'utiliser

- **Contrôle du PC :** une fois reliée à une IA, elle peut voir l'écran et utiliser la souris et le clavier. Capri FR démarre en mode **Guarded**, qui demande avant la plupart des actions. Le réglage se trouve dans Mainframe > Réglages > HABITUDES > Sécurité.
- **À éviter :** le mode Blackwall et les mods de code, qui ont un accès complet au PC. N'installe que des mods en lesquels tu as confiance.
- **Démarrage de Windows :** une copie installée se lance au démarrage. Tu peux le couper dans Mainframe > Réglages > HABITUDES > Lancer avec Windows.
- **Pas de mise à jour automatique :** les correctifs de sécurité de Capri n'arrivent pas tout seuls. Reviens sur cette page pour installer les nouvelles versions de Capri FR.
- **Clés API :** si tu relies un service d'IA en ligne, tes échanges passent par ce service, selon ses propres conditions.
- **Désinstallation :** elle retire le programme, ses modèles, ses raccourcis et le lancement au démarrage, mais **garde tes discussions, ta mémoire et tes réglages** dans `%LOCALAPPDATA%\CapriFR`. Pour tout effacer, supprime ce dossier après la désinstallation.

## Ce qui change par rapport à Capri

Capri FR repose sur **Capri v0.8.3**. Elle ne contient pas les corrections et nouveautés publiées par Capri depuis, et le code français se trouve sur la branche `fr` de ce dépôt.

- Interface, réponses d'aide, messages et mini-jeux en français, avec le français comme langue par défaut.
- Voix française par les voix de Windows, voix de femme en priorité.
- Sécurité **Guarded** par défaut, au lieu de Trusted.
- Mises à jour automatiques coupées, car celles de Capri remplaceraient cette version.
- Nom, fichiers et dossier de données distincts (`CapriFR.exe`, `%LOCALAPPDATA%\CapriFR`), pour cohabiter avec une vraie Capri.
- La pastille ronde et la barre de saisie cyan, reprises de la démo du site de Capri.

**Encore en anglais :** certaines fiches d'aide (« What's new » et les fiches réservées au modèle), des messages techniques rares, les noms de couleurs des mods et les préréglages de sécurité. Les instructions destinées au modèle d'IA restent en anglais, et il répond en français. Certaines fiches d'aide ont été condensées par rapport à l'original.

**Tests :** la suite de tests complète de Capri (`--smoke-test`) n'a pas été adaptée à cette version, donc certaines de ses vérifications ne correspondent plus. L'installation, le démarrage et la désinstallation ont été vérifiés sous Windows 11 uniquement. Les versions Mac et Linux et l'application Quest présentes dans ce dépôt n'ont pas été traduites ni testées.

## Problèmes

Signale les problèmes de Capri FR dans les [issues de ce dépôt](../../issues), pas auprès du projet Capri.

## Compiler

```powershell
powershell -ExecutionPolicy Bypass -File ci\build-windows.ps1
```

Il faut Windows, le .NET Framework 4.8 et le SDK Windows. Le résultat se trouve dans `out\windows`.

## Licence et crédits

- **Code, dessins et animations :** projet [Capri](https://github.com/lachydotmcg/capri-desktop) de Lachy, avec le travail de « Capri's artist », sous licence [PolyForm Noncommercial 1.0.0](LICENSE.md).
- **Usage non commercial uniquement :** pas de vente, de version payante ni d'usage commercial. En cas de partage, garde ce crédit, la licence et les lignes `Required Notice`.
- **Composants tiers :** voir [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md) et la liste du [README d'origine](README-Capri-original.md).
- **Création de fans :** Capri est un projet de fans non officiel, qui n'est ni approuvé ni soutenu par CD PROJEKT RED, les ayants droit de l'anime ou ses comédiens de doublage. La licence ne donne aucun droit sur ce qui leur appartient.