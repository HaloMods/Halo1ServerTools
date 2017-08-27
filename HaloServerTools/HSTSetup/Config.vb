Imports System.IO
Imports Nini.Config

Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        ReadConfigData()
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Wizard1 As WizardControl.Wizard
    Friend WithEvents WizardPage1 As WizardControl.WizardPage
    Friend WithEvents WizardPage2 As WizardControl.WizardPage
    Friend WithEvents WizardPage3 As WizardControl.WizardPage
    Friend WithEvents label7 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseInstalledFolder As System.Windows.Forms.Button
    Friend WithEvents label3 As System.Windows.Forms.Label
    Friend WithEvents txtInstalledFolder As System.Windows.Forms.TextBox
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDataFolder As System.Windows.Forms.Button
    Friend WithEvents label8 As System.Windows.Forms.Label
    Friend WithEvents txtDataFolder As System.Windows.Forms.TextBox
    Friend WithEvents label6 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents label5 As System.Windows.Forms.Label
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents label4 As System.Windows.Forms.Label
    Friend WithEvents WizardPage4 As WizardControl.WizardPage
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnAutoScan As System.Windows.Forms.Button
    Friend WithEvents txtLogfile As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Form1))
        Me.Wizard1 = New WizardControl.Wizard
        Me.WizardPage1 = New WizardControl.WizardPage
        Me.label7 = New System.Windows.Forms.Label
        Me.btnBrowseInstalledFolder = New System.Windows.Forms.Button
        Me.label3 = New System.Windows.Forms.Label
        Me.txtInstalledFolder = New System.Windows.Forms.TextBox
        Me.WizardPage2 = New WizardControl.WizardPage
        Me.label1 = New System.Windows.Forms.Label
        Me.btnBrowseDataFolder = New System.Windows.Forms.Button
        Me.label8 = New System.Windows.Forms.Label
        Me.txtDataFolder = New System.Windows.Forms.TextBox
        Me.WizardPage3 = New WizardControl.WizardPage
        Me.label6 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.label5 = New System.Windows.Forms.Label
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.label4 = New System.Windows.Forms.Label
        Me.WizardPage4 = New WizardControl.WizardPage
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtLogfile = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.btnAutoScan = New System.Windows.Forms.Button
        Me.Wizard1.SuspendLayout()
        Me.WizardPage1.SuspendLayout()
        Me.WizardPage2.SuspendLayout()
        Me.WizardPage3.SuspendLayout()
        Me.WizardPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Wizard1
        '
        Me.Wizard1.BannerBitmap = CType(resources.GetObject("Wizard1.BannerBitmap"), System.Drawing.Image)
        Me.Wizard1.Controls.Add(Me.WizardPage1)
        Me.Wizard1.Controls.Add(Me.WizardPage2)
        Me.Wizard1.Controls.Add(Me.WizardPage3)
        Me.Wizard1.Controls.Add(Me.WizardPage4)
        Me.Wizard1.Location = New System.Drawing.Point(0, 0)
        Me.Wizard1.Name = "Wizard1"
        Me.Wizard1.Size = New System.Drawing.Size(497, 360)
        Me.Wizard1.TabIndex = 0
        Me.Wizard1.Title = "HST Configuration"
        Me.Wizard1.WelcomeBitmap = CType(resources.GetObject("Wizard1.WelcomeBitmap"), System.Drawing.Image)
        Me.Wizard1.WelcomeText = "This wizard will guide you through setting up and configuring a new HST install."
        '
        'WizardPage1
        '
        Me.WizardPage1.Controls.Add(Me.label7)
        Me.WizardPage1.Controls.Add(Me.btnBrowseInstalledFolder)
        Me.WizardPage1.Controls.Add(Me.label3)
        Me.WizardPage1.Controls.Add(Me.txtInstalledFolder)
        Me.WizardPage1.Description = "Dedicated Server Process"
        Me.WizardPage1.Heading = "File Locations"
        Me.WizardPage1.Location = New System.Drawing.Point(21, 71)
        Me.WizardPage1.Name = "WizardPage1"
        Me.WizardPage1.Size = New System.Drawing.Size(456, 230)
        Me.WizardPage1.TabIndex = 0
        '
        'label7
        '
        Me.label7.Location = New System.Drawing.Point(23, 56)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(392, 13)
        Me.label7.TabIndex = 12
        Me.label7.Text = "Click Browse to choose a folder"
        '
        'btnBrowseInstalledFolder
        '
        Me.btnBrowseInstalledFolder.Location = New System.Drawing.Point(359, 72)
        Me.btnBrowseInstalledFolder.Name = "btnBrowseInstalledFolder"
        Me.btnBrowseInstalledFolder.TabIndex = 11
        Me.btnBrowseInstalledFolder.Text = "Browse"
        '
        'label3
        '
        Me.label3.Location = New System.Drawing.Point(22, 8)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(392, 32)
        Me.label3.TabIndex = 10
        Me.label3.Text = "Please choose the folder where halocedex.exe is installed.  Once you have located" & _
        " the folder, click Next to continue."
        '
        'txtInstalledFolder
        '
        Me.txtInstalledFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInstalledFolder.Location = New System.Drawing.Point(23, 72)
        Me.txtInstalledFolder.Name = "txtInstalledFolder"
        Me.txtInstalledFolder.Size = New System.Drawing.Size(328, 20)
        Me.txtInstalledFolder.TabIndex = 9
        Me.txtInstalledFolder.Text = ""
        '
        'WizardPage2
        '
        Me.WizardPage2.Controls.Add(Me.label1)
        Me.WizardPage2.Controls.Add(Me.btnBrowseDataFolder)
        Me.WizardPage2.Controls.Add(Me.label8)
        Me.WizardPage2.Controls.Add(Me.txtDataFolder)
        Me.WizardPage2.Description = "Banlist and Logfile"
        Me.WizardPage2.Heading = "File Locations"
        Me.WizardPage2.Location = New System.Drawing.Point(21, 71)
        Me.WizardPage2.Name = "WizardPage2"
        Me.WizardPage2.Size = New System.Drawing.Size(456, 230)
        Me.WizardPage2.TabIndex = 1
        '
        'label1
        '
        Me.label1.Location = New System.Drawing.Point(23, 56)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(392, 13)
        Me.label1.TabIndex = 19
        Me.label1.Text = "Click Browse to choose a folder"
        '
        'btnBrowseDataFolder
        '
        Me.btnBrowseDataFolder.Location = New System.Drawing.Point(359, 72)
        Me.btnBrowseDataFolder.Name = "btnBrowseDataFolder"
        Me.btnBrowseDataFolder.TabIndex = 18
        Me.btnBrowseDataFolder.Text = "Browse"
        '
        'label8
        '
        Me.label8.Location = New System.Drawing.Point(22, 8)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(392, 32)
        Me.label8.TabIndex = 17
        Me.label8.Text = "Please choose the folder where the banlist and logfile are stored.  Once you have" & _
        " located the folder, click Next to continue."
        '
        'txtDataFolder
        '
        Me.txtDataFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataFolder.Location = New System.Drawing.Point(23, 72)
        Me.txtDataFolder.Name = "txtDataFolder"
        Me.txtDataFolder.Size = New System.Drawing.Size(328, 20)
        Me.txtDataFolder.TabIndex = 16
        Me.txtDataFolder.Text = ""
        '
        'WizardPage3
        '
        Me.WizardPage3.Controls.Add(Me.label6)
        Me.WizardPage3.Controls.Add(Me.txtPort)
        Me.WizardPage3.Controls.Add(Me.label5)
        Me.WizardPage3.Controls.Add(Me.txtIP)
        Me.WizardPage3.Controls.Add(Me.label4)
        Me.WizardPage3.Description = "IP Address and Port"
        Me.WizardPage3.Heading = "Configuration"
        Me.WizardPage3.Location = New System.Drawing.Point(21, 71)
        Me.WizardPage3.Name = "WizardPage3"
        Me.WizardPage3.Size = New System.Drawing.Size(456, 230)
        Me.WizardPage3.TabIndex = 2
        '
        'label6
        '
        Me.label6.Location = New System.Drawing.Point(160, 56)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(48, 16)
        Me.label6.TabIndex = 14
        Me.label6.Text = "Port"
        '
        'txtPort
        '
        Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPort.Location = New System.Drawing.Point(160, 72)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(56, 20)
        Me.txtPort.TabIndex = 13
        Me.txtPort.Text = "2302"
        '
        'label5
        '
        Me.label5.Location = New System.Drawing.Point(24, 56)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(72, 16)
        Me.label5.TabIndex = 12
        Me.label5.Text = "IP Address"
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(24, 72)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(120, 20)
        Me.txtIP.TabIndex = 11
        Me.txtIP.Text = ""
        '
        'label4
        '
        Me.label4.Location = New System.Drawing.Point(24, 8)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(408, 32)
        Me.label4.TabIndex = 10
        Me.label4.Text = "HST needs to know the IP address and port that your server will be running on, so" & _
        " that the process command line can be constructed."
        '
        'WizardPage4
        '
        Me.WizardPage4.Controls.Add(Me.btnAutoScan)
        Me.WizardPage4.Controls.Add(Me.Label9)
        Me.WizardPage4.Controls.Add(Me.txtLogfile)
        Me.WizardPage4.Controls.Add(Me.Label10)
        Me.WizardPage4.Description = "Logfile Name"
        Me.WizardPage4.Heading = "Configuration"
        Me.WizardPage4.Location = New System.Drawing.Point(21, 71)
        Me.WizardPage4.Name = "WizardPage4"
        Me.WizardPage4.Size = New System.Drawing.Size(456, 230)
        Me.WizardPage4.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(24, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 16)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Logfile Name"
        '
        'txtLogfile
        '
        Me.txtLogfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLogfile.Location = New System.Drawing.Point(24, 72)
        Me.txtLogfile.Name = "txtLogfile"
        Me.txtLogfile.Size = New System.Drawing.Size(200, 20)
        Me.txtLogfile.TabIndex = 16
        Me.txtLogfile.Text = ""
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(24, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(408, 40)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Fill in the name of the logfile that HST will be reading from and Click Next.  To" & _
        " have Setup scan the HaloCE 'init.txt' file and attempt to automatically determi" & _
        "ne the name, click Auto-Scan"
        '
        'btnAutoScan
        '
        Me.btnAutoScan.Location = New System.Drawing.Point(232, 72)
        Me.btnAutoScan.Name = "btnAutoScan"
        Me.btnAutoScan.TabIndex = 19
        Me.btnAutoScan.Text = "Auto-Scan"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(496, 358)
        Me.Controls.Add(Me.Wizard1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.Wizard1.ResumeLayout(False)
        Me.WizardPage1.ResumeLayout(False)
        Me.WizardPage2.ResumeLayout(False)
        Me.WizardPage3.ResumeLayout(False)
        Me.WizardPage4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Shared Sub Main()
        Dim f As New Form1
        f.ShowDialog()
    End Sub

    Private installedFolder As String
    Private dataFolder As String
    Private ipAddress As String
    Private port As String
    Private logfile As String

    Private Sub btnBrowseInstalledFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseInstalledFolder.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Choose the folder that contains the haloceded.exe file, and click OK."
        If fbd.ShowDialog = DialogResult.Cancel Then Exit Sub
        txtInstalledFolder.Text = fbd.SelectedPath
    End Sub
    Private Sub btnBrowseDataFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDataFolder.Click
        Dim fbd As New FolderBrowserDialog
        fbd.Description = "Choose the folder that contains the haloceded.exe file, and click OK."
        If fbd.ShowDialog = DialogResult.Cancel Then Exit Sub
        txtDataFolder.Text = fbd.SelectedPath
    End Sub
    Private Sub Wizard1_ValidatePage(ByVal sender As System.Object, ByVal e As WizardControl.WizardPageValidateEventArgs) Handles Wizard1.ValidatePage
        If e.Page Is WizardPage1 Then
            Dim s As String = txtInstalledFolder.Text
            If File.Exists(s + "\haloceded.exe") Then
                e.Valid = True
                installedFolder = txtInstalledFolder.Text
            Else
                MessageBox.Show("File not found in the selected folder!" & vbCrLf & s & "\haloceded.exe", _
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Valid = False
            End If
        End If
        If e.Page Is WizardPage2 Then
            If txtDataFolder.Text = "" Then
                MessageBox.Show("You must choose a folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Valid = False
                Exit Sub
            End If
            If Not Directory.Exists(txtDataFolder.Text) Then
                MessageBox.Show("The specified folder is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Valid = False
                Exit Sub
            End If
            dataFolder = txtDataFolder.Text
        End If
        If e.Page Is WizardPage3 Then
            If txtIP.Text = "" Or txtPort.Text = "" Then
                MessageBox.Show("You must fill in an the required data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Valid = False
                Exit Sub
            End If
            ipAddress = txtIP.Text
            port = txtPort.Text
            e.Valid = True
        End If
        If e.Page Is WizardPage4 Then
            If txtLogfile.Text = "" Then
                MessageBox.Show("You must enter a logfile name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Valid = False
                Exit Sub
            End If
            logfile = txtLogfile.Text
            e.Valid = True
        End If
    End Sub

    Private Sub WriteConfigData(ByVal sender As System.Object, ByVal e As WizardControl.WizardPageSummaryEventArgs) Handles Wizard1.BeforeSummaryPageDisplayed
        'Wizard has been completed - this is where we will write
        'the data to the configuration file
        If Not Directory.Exists(Application.StartupPath & "\settings") Then
            Directory.CreateDirectory(Application.StartupPath & "\settings")
        End If
        Dim p As String = Application.StartupPath & "\settings\settings.ini"
        If File.Exists(p) Then File.Delete(p)
        Dim fs As FileStream = File.Create(p)
        fs.Close()
        Dim source As IConfigSource = New IniConfigSource(p)

        'Add the paths section first
        Dim pathsConfig As IConfig = source.AddConfig("Paths")
        pathsConfig.Set("InstalledFolder", installedFolder)
        pathsConfig.Set("DataFolder", dataFolder)

        'Next add the IP/Port
        Dim settingsConfig As IConfig = source.AddConfig("Settings")
        settingsConfig.Set("IPAddress", ipAddress)
        settingsConfig.Set("Port", port)

        'Next add required filenames
        Dim filenamesConfig As IConfig = source.AddConfig("Filenames")
        filenamesConfig.Set("Logfile", logfile)

        source.Save()
    End Sub

    Private Sub ReadConfigData()
        Dim p As String = Application.StartupPath & "\settings\settings.ini"
        If Not File.Exists(p) Then Exit Sub
        Dim source As IConfigSource = New IniConfigSource(p)

        'Read the paths section first
        Dim pathsConfig As IConfig = source.Configs("Paths")
        installedFolder = pathsConfig.GetString("InstalledFolder")
        dataFolder = pathsConfig.GetString("DataFolder")

        'Next read the IP/Port
        Dim settingsConfig As IConfig = source.Configs("Settings")
        ipAddress = settingsConfig.GetString("IPAddress")
        port = settingsConfig.GetString("Port")

        'Next read required filenames
        Dim filenamesConfig As IConfig = source.Configs("Filenames")
        logfile = filenamesConfig.GetString("Logfile")

        'Fill in the controls with the data
        txtInstalledFolder.Text = installedFolder
        txtDataFolder.Text = dataFolder
        txtIP.Text = ipAddress
        txtPort.Text = port
        txtLogfile.Text = logfile
    End Sub

    Private Sub btnAutoScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoScan.Click
        Dim init As String = installedFolder + "\init.txt"
        If Not File.Exists(init) Then
            MessageBox.Show("Init file was not found - unable to auto-fill.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim sr As New StreamReader(New FileStream(init, FileMode.Open))
        Dim text As String = sr.ReadToEnd()
        text = text.Replace(vbCrLf, vbLf)

        Dim lines As String() = text.Split(vbLf)
        For x As Integer = 0 To lines.Length - 1
            'sv_log_file "name"
            Dim temp As String = lines(x)
            If temp.StartsWith("sv_log_file") Then
                temp = temp.Replace("sv_log_file ", "")
                temp = temp.Replace("""", "")
                txtLogfile.Text = temp + ".log"
                Exit Sub
            End If
        Next
        MessageBox.Show("Logfile entry was not found in the init.txt file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class
