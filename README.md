# NNTP Project 2025
En WPF-baseret newsreader-applikation, der kommunikerer med en NNTP-server (Network News Transport Protocol).  
Projektet er udviklet som en del af Datamatikeruddannelsen på EASV med fokus på TCP-klient/server-kommunikation, MVVM-arkitektur og Personal eXtreme Programming (PXP).

## Formål

Formålet med projektet er at demonstrere, hvordan en klient kan:
- Oprette forbindelse til en NNTP-server via TCP
- Hente og vise en liste over nyhedsgrupper
- Vise artikeloverskrifter (headlines)
- Hente og vise fulde artikler

Applikationen fungerer som en mini-"Outlook for newsgroups", hvor brugeren kan navigere i grupper og artikler direkte fra GUI’en.

## Teknologier
- Sprog: C#
- Framework: .NET WPF
- Designmønster: MVVM (Model–View–ViewModel)
- Test: Enhedstest med TDD (Test First-principper)
- Protokol: NNTP over TCP
- Versionsstyring: Git og GitHub
- IDE: Visual Studio 2022

## Systemstruktur
Projektet består af tre hoveddele:

| Lag | Projekt | Funktion |
|-----|----------|-----------|
| Præsentation | Nntp.Client.Wpf | GUI bygget i WPF med Views, ViewModels og eventhåndtering |
| Logik & Forbindelse | Nntp.Core | Håndtering af NNTP-protokollen (LIST, GROUP, XOVER, ARTICLE) |
| Tests | Nntp.Tests | Enhedstest og test-doubles (FakeTransport) til klientlogik |

## Funktionalitet
Implementerede user stories (US1–US4):

| # | Beskrivelse | Status |
|---|--------------|--------|
| US1 | Enter and save server information | Færdig |
| US2 | Download list of newsgroups | Færdig |
| US3 | Select newsgroup and read article headlines | Færdig |
| US4 | Select headline and read full article | Færdig |
| US5–US9 | Udvidelser (posting, søgning, favoritter) | Planlagt |

## Skærmbilleder
Applikationens GUI består af tre hovedpaneler:
- Venstre panel: Nyhedsgrupper  
- Midterpanel: Artikeloverskrifter  
- Højre panel: Fuld artikeltekst  

## Udviklingsmetode

Projektet anvender Personal eXtreme Programming (PXP):
- Iterativ udvikling i korte forløb  
- Test First-principper (TDD)  
- Refactoring og løbende forbedringer  
- Små releases og synlig fremdrift (Kanban-board)
