﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Savas.Library.Enum;
using Savas.Library.Interface;

namespace Savas.Library.Concrete
{
    public class Oyun : IOyun
    {
        #region Alanlar

        private readonly Timer _gecenSureTimer = new Timer { Interval = 1000 };
        private TimeSpan _gecenSure;
        private readonly Panel _ucaksavarPanel;
        private readonly Panel _savasAlaniPanel;
        private Ucaksavar _ucaksavar;
        private readonly List<Mermi> _mermiler = new List<Mermi>();

        #endregion

        #region Olaylar

        public event EventHandler GecenSureDegisti;

        #endregion

        #region Özellikler

        public bool DevamEdiyorMu { get; private set; }

        public TimeSpan GecenSure
        {
            get => _gecenSure;
            private set
            {
                _gecenSure = value;

                GecenSureDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Metotlar

        public Oyun(Panel ucaksavarPanel, Panel savasAlaniPanel)
        {
            _ucaksavarPanel = ucaksavarPanel;
            _savasAlaniPanel = savasAlaniPanel;
            _gecenSureTimer.Tick += GecenSureTimer_Tick;
        }

        private void GecenSureTimer_Tick(object sender, EventArgs e)
        {
            GecenSure += TimeSpan.FromSeconds(1);
        }

        public void Baslat()
        {
            if (DevamEdiyorMu) return;

            DevamEdiyorMu = true;
            _gecenSureTimer.Start();

            UcaksavarOlustur();
        }

        private void UcaksavarOlustur()
        {
            _ucaksavar = new Ucaksavar(_ucaksavarPanel.Width, _ucaksavarPanel.Size);
            _ucaksavarPanel.Controls.Add(_ucaksavar);
        }

        private void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;
            _gecenSureTimer.Stop();
        }

        public void AtesEt()
        {
            if (!DevamEdiyorMu) return;

            var mermi = new Mermi(_savasAlaniPanel.Size, _ucaksavar.Center);
            _mermiler.Add(mermi);
            _savasAlaniPanel.Controls.Add(mermi);
        }

        public void UcaksavariHareketEttir(Yon yon)
        {
            if (!DevamEdiyorMu) return;

            _ucaksavar.HareketEttir(yon);
        }

        #endregion
    }
}
