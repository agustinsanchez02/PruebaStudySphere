﻿using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Web.ModelBinding;
using COMUN;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using FontAwesome.Sharp;
using System.Reflection;
using System.Runtime.InteropServices;
using Dominios;
using Representacion.Formularios;
using CapaSoporte.Caché;

namespace Representacion
{
    public partial class iniciar_sesion : Form
    {
        public iniciar_sesion()
        {
            InitializeComponent();
        }
        int bandera = 1;

        [DllImport("User32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("User32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        private void msgError(string msg)
        {
            lblErrorMensaje.Text = "      " + msg;
            lblErrorMensaje.Visible = true;
            lblErrorMensaje.ForeColor = Color.White;
        }
        private void Contraseña_Enter(object sender, EventArgs e)
        {
            if (Contraseña.Text == "Contraseña")
            {
                Contraseña.Text = "";
                Contraseña.ForeColor = Color.White;
                Contraseña.UseSystemPasswordChar = true;
            }
        }
        private void Contraseña_Leave(object sender, EventArgs e)
        {
            if (Contraseña.Text == "")
            {
                Contraseña.Text = "Contraseña";
                Contraseña.ForeColor = Color.Gray;
                Contraseña.UseSystemPasswordChar = false;
            }
        }
        private void Usuario_Enter(object sender, EventArgs e)
        {
            if (Usuario.Text == "Usuario/Mail")
            {
                Usuario.Text = "";
                Usuario.ForeColor = Color.White;
            }
        }
        private void Usuario_Leave(object sender, EventArgs e)
        {
            if (Usuario.Text == "")
            {
                Usuario.Text = "Usuario/Mail";
                Usuario.ForeColor = Color.Gray;
            }
        }
        public void CerrarSesion(object sender, FormClosedEventArgs e)
        {
            Contraseña.Text = "Contraseña";
            Contraseña.ForeColor = Color.Gray;
            Contraseña.UseSystemPasswordChar = false;
            Usuario.Text = "Usuario/Mail";
            Usuario.ForeColor = Color.Gray;
            lblErrorMensaje.Visible = false;
            this.Show();
        }
        private void IniciarSesionbtn_Click(object sender, EventArgs e)
        {
            if (Usuario.Text != "Usuario/Mail")
            {
                if (Contraseña.Text != "Contraseña")
                {
                    ModeloUsuario usuario = new ModeloUsuario();
                    var loginvalido = usuario.iniciosesion(Usuario.Text, Contraseña.Text);
                    if (loginvalido == true)
                    {
                        this.Hide();
                        Bienvenida bienvenida = new Bienvenida();
                        bienvenida.ShowDialog();
                        if (CacheUsuario.Privilegios == 0)
                        {
                            VistaAdministrador vistaAdministrador = new VistaAdministrador();
                            vistaAdministrador.Show();
                        }
                        else
                        {
                            Inicio inicio = new Inicio();
                            inicio.Show();
                            inicio.FormClosed += CerrarSesion;
                        }

                    }
                    else
                    {
                        msgError("El usuario o la contraseña son incorrectos. \n      Por favor intente otra vez.");
                        Contraseña.Text = "Contraseña";
                        Contraseña.ForeColor = Color.Gray;
                        Contraseña.UseSystemPasswordChar=false;
                        Contraseña.Focus();

                    }
                }
                else msgError("Por favor ingresa una contraseña.");
            }
            else
            {
                msgError("Por favor Ingresa un Usuario o Mail.");
            }
        }

        private void Cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iniciar_sesion_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            iniciar_sesion_MouseDown((object)sender, e);
        }

        private void Ojo_Click(object sender, EventArgs e)
        {
            {
                if (bandera == 0)
                {
                    Ojo.BackgroundImage = Properties.Resources.Ojo_cerrado;
                    Contraseña.UseSystemPasswordChar = true;
                    bandera = 1;
                    Ojo.Height = 30;
                    Ojo.Width = 26;
                }
                else
                {
                    Ojo.BackgroundImage = Properties.Resources.Ojoabierto;
                    bandera = 0;
                    Contraseña.UseSystemPasswordChar = false;
                    Ojo.Width = 25;
                    Ojo.Height = 25;

                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CorreoRecuperoPass recuperoPass = new CorreoRecuperoPass();
            recuperoPass.Show();
            this.Hide();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registro registro = new Registro();
            registro.Show();

        }

    }
}

