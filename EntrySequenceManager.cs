using GTA;
using GTA.Math;
using GTA.Native;
using LemonUI;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Reflection;

public class EntrySequenceManager : Script
{
    // LemonUI nesne havuzu
    private readonly ObjectPool uiPool = new ObjectPool();

    // Ana karakter ve NPC referansları
    private Ped patron;
    private Ped secretary;
    private Ped securityChief;
    private List<Ped> securityGuards = new List<Ped>();
    private Ped driver;
    private Vehicle jet;
    private Vehicle patronCar;
    private List<Vehicle> escortCars = new List<Vehicle>();

    // LemonUI menüsü (sekreter bilgilendirmesi)
    private NativeMenu introDialogMenu;

    public EntrySequenceManager()
    {
        // Tick eventleri SHVDN3 uyumlu olarak bağlanır
        Tick += OnTick;
        KeyDown += OnKeyDown;

        SetupMenus();
    }

    private void SetupMenus()
    {
        introDialogMenu = new NativeMenu("Bilgilendirme", "Sekreterinizin Günlük Özeti");
        introDialogMenu.Add(new NativeItem("Patron, Los Santos’a hoş geldiniz! Bugün teslimat, VIP gece kulübü, MC toplantısı var..."));
        uiPool.Add(introDialogMenu);
    }

    // Oyun açılış senaryosu başlatılır
    public void StartEntrySequence()
    {
        SpawnPrivateJet();
        SpawnNPCsInJet();
        PlayJetLandingSequence();
        ShowSecretaryIntroDialog();
        AnimateJetExit();
        AnimateLuggageTransfer();
        AnimateCarBoarding();
        StartConvoyToOffice();
        ShowSecretaryRoadBriefing();
        AnimateOfficeArrival();
        AnimateElevatorToOffice();
        InitializeGameSystems();
    }

    private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        // Test için F6 tuşu ile açılış sekansı tetiklenir
        if (e.KeyCode == System.Windows.Forms.Keys.F6)
        {
            StartEntrySequence();
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        // LemonUI menüleri ve cutscene ilerletmeleri burada çalışır
        uiPool.Process();
    }

    // --- Senaryonun modüler adım fonksiyonları ---
    private void SpawnPrivateJet()
    {
        // ScriptHookVDotNet3 uyumlu jet spawn (örnek model: Luxor)
        Vector3 jetPos = new Vector3(-1652.0f, -3143.0f, 13.0f); // LSIA
        jet = World.CreateVehicle(new Model(VehicleHash.Luxor), jetPos);
        jet.Heading = 90.0f;
    }

    private void SpawnNPCsInJet()
    {
        // Patron karakteri ve NPC'leri oluştur (SHVDN3)
        patron = Game.Player.Character;
        Game.Player.Character.Position = jet.GetOffsetPosition(new Vector3(1.0f, -2.0f, 1.0f));
        Game.Player.Character.SetIntoVehicle(jet, VehicleSeat.Driver);

        secretary = World.CreatePed(new Model(PedHash.Business01AFY), jet.GetOffsetPosition(new Vector3(1.0f, -1.0f, 1.0f)));
        securityChief = World.CreatePed(new Model(PedHash.Security01SMM), jet.GetOffsetPosition(new Vector3(-1.0f, -1.0f, 1.0f)));
        secretary.SetIntoVehicle(jet, VehicleSeat.Passenger);
        securityChief.SetIntoVehicle(jet, VehicleSeat.RightFront);
    }

    private void PlayJetLandingSequence()
    {
        // Jet pistte ilerler, ScriptHookVDotNet3 ile kısa cutscene (örnek olarak hız azaltılır)
        jet.Speed = 20.0f;
        jet.Position = new Vector3(-1652.0f, -3143.0f, 13.0f); // Pist başı

        // Hatalı satır kaldırıldı: jet.Task.DriveTo(...)
        // Alternatif olarak, jet'in pozisyonunu veya hızını OnTick içinde kademeli olarak değiştirebilirsiniz.
        // Örneğin:
        // Vector3 hedefPos = jet.Position + jet.ForwardVector * 100.0f;
        // jet.Position = hedefPos; // Jet'i anında hedefe taşır
    }

    private void ShowSecretaryIntroDialog()
    {
        // LemonUI ile sekreter diyalog menüsü açılır
        introDialogMenu.Visible = true;
    }

    private void AnimateJetExit()
    {
        // NPC'ler uçaktan inmek için pozisyonlanır (SHVDN3 uyumlu)
        secretary.Task.LeaveVehicle();
        securityChief.Task.LeaveVehicle();
        patron.Task.LeaveVehicle();
    }

    private void AnimateLuggageTransfer()
    {
        // Valiz ve evrak objeleri spawn ve araca yükleme için placeholder
        // (Sonraki adımda asset ile düzenlenebilir)
    }

    private void AnimateCarBoarding()
    {
        // Patron, sekreter ve güvenlik müdürü araçlara biner
        // Araç spawn ve NPC bindirme kodları modüler şekilde ayrılacak
    }

    private void StartConvoyToOffice()
    {
        // Araçlar konvoy halinde yönetim merkezine gider (SHVDN3 uyumlu)
    }

    private void ShowSecretaryRoadBriefing()
    {
        // Araç içi LemonUI bilgilendirme menüsü açılır
    }

    private void AnimateOfficeArrival()
    {
        // Ofise varış ve araçlardan inme kodları
    }

    private void AnimateElevatorToOffice()
    {
        // Asansörle ofis katına çıkış ve kamera geçişleri
    }

    private void InitializeGameSystems()
    {
        // Diğer ana sistemler ve menüler burada aktive edilir
    }
}