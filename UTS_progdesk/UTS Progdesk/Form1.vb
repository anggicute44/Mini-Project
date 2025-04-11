Imports System.IO

Public Class Form1
    Public Class Mahasiswa
        Public Property NIM As String
        Public Property Nama As String
        Public Property IPK As String
        Public Property Foto As String
    End Class

    Dim DaftarMahasiswa As New List(Of Mahasiswa) ' List untuk menyimpan semua data mahasiswa
    Dim indeksAktif As Integer = 0 ' Menyimpan indeks data mahasiswa yang sedang ditampilkan
    Dim fotoBaruPath As String = "" ' Menyimpan nama file foto yang dipilih user
    Dim fileIniPath As String = "D:\UTS_progdesk\bahan_uts\dataNIM.ini" ' Path lokasi file INI
    Dim folderFoto As String = "D:\UTS_progdesk\bahan_uts\" ' Folder tempat menyimpan foto
    Dim modeTambah As Boolean = False ' True saat tambah data, False saat edit

    Private Sub LoadDataFromIni(ByVal filePath As String)
        DaftarMahasiswa.Clear()
        Dim current As New Mahasiswa()

        For Each line As String In File.ReadAllLines(filePath)
            If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                If current.NIM IsNot Nothing Then
                    DaftarMahasiswa.Add(current)
                    current = New Mahasiswa()
                End If
            ElseIf line.Contains("=") Then
                Dim parts = line.Split("="c)
                Dim key = parts(0).Trim().ToLower()
                Dim value = parts(1).Trim()

                Select Case key
                    Case "nim" : current.NIM = value
                    Case "nama" : current.Nama = value
                    Case "ipk" : current.IPK = value
                    Case "foto" : current.Foto = value
                End Select
            End If
        Next

        If current.NIM IsNot Nothing Then
            DaftarMahasiswa.Add(current)
        End If

        btnNext.Enabled = True
        btnPrevious.Enabled = True
        btnLast.Enabled = True
        btnFirst.Enabled = True
        btnDelete.Enabled = True
        btnSearch.Enabled = True
        btnEdit.Enabled = True
        tbNIM.ReadOnly = False
    End Sub

    Private Sub SimpanKeFileIni(ByVal filePath As String)
        Using writer As New StreamWriter(filePath, False)
            For Each mhs As Mahasiswa In DaftarMahasiswa
                writer.WriteLine("[" & mhs.NIM & "]")
                writer.WriteLine("nim=" & mhs.NIM)
                writer.WriteLine("nama=" & mhs.Nama)
                writer.WriteLine("ipk=" & mhs.IPK)
                writer.WriteLine("foto=" & mhs.Foto)
            Next
        End Using
    End Sub

    Private Sub Tampil()
        If DaftarMahasiswa.Count = 0 Then Return

        Dim mhs = DaftarMahasiswa(indeksAktif)
        tbNimInGB.Text = mhs.NIM
        tbNamaInGB.Text = mhs.Nama
        tbIpkInGB.Text = mhs.IPK

        Dim imagePath As String = Path.Combine(folderFoto, mhs.Foto)
        If File.Exists(imagePath) Then
            Using imgTemp As Image = Image.FromFile(imagePath)
                PictureBox1.Image = New Bitmap(imgTemp)
            End Using
        Else
            PictureBox1.Image = Nothing
        End If
    End Sub

    Private Sub btnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
        btnLoadData.Enabled = False
        LoadDataFromIni(fileIniPath)
        indeksAktif = 0
        Tampil()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If indeksAktif < DaftarMahasiswa.Count - 1 Then
            indeksAktif += 1
            Tampil()
        Else
            MessageBox.Show("Sudah mencapai data terakhir.")
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If indeksAktif > 0 Then
            indeksAktif -= 1
            Tampil()
        Else
            MessageBox.Show("Sudah mencapai data pertama.")
        End If
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        indeksAktif = DaftarMahasiswa.Count - 1
        Tampil()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        indeksAktif = 0
        Tampil()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        btnEdit.Enabled = False
        btnEdit.Visible = False
        btnSave.Enabled = True
        btnSave.Visible = True

        tbNimInGB.ReadOnly = False
        tbNamaInGB.ReadOnly = False
        tbIpkInGB.ReadOnly = False

        btnNext.Enabled = False
        btnPrevious.Enabled = False
        btnFirst.Enabled = False
        btnLast.Enabled = False
        btnDelete.Enabled = False
        btnSearch.Enabled = False

        btnBrowse.Enabled = True
        btnBrowse.Visible = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If modeTambah Then
            ' Mode tambah data baru
            Dim mhsBaru As New Mahasiswa()
            mhsBaru.NIM = tbNimInGB.Text.Trim()
            mhsBaru.Nama = tbNamaInGB.Text.Trim()
            mhsBaru.IPK = tbIpkInGB.Text.Trim()
            mhsBaru.Foto = fotoBaruPath

            If mhsBaru.NIM = "" OrElse mhsBaru.Nama = "" Then
                MessageBox.Show("NIM dan Nama wajib diisi.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            DaftarMahasiswa.Add(mhsBaru)
            indeksAktif = DaftarMahasiswa.Count - 1
            SimpanKeFileIni(fileIniPath)
            modeTambah = False

        ElseIf indeksAktif >= 0 AndAlso indeksAktif < DaftarMahasiswa.Count Then
            ' Mode edit data lama
            Dim mhs = DaftarMahasiswa(indeksAktif)
            mhs.NIM = tbNimInGB.Text
            mhs.Nama = tbNamaInGB.Text
            mhs.IPK = tbIpkInGB.Text

            If fotoBaruPath <> "" Then
                mhs.Foto = fotoBaruPath
                fotoBaruPath = ""
            End If

            SimpanKeFileIni(fileIniPath)
        End If

        ' Reset UI setelah menyimpan
        btnEdit.Enabled = True
        btnEdit.Visible = True
        btnSave.Enabled = False
        btnSave.Visible = False
        btnBrowse.Enabled = False
        btnBrowse.Visible = False

        tbNimInGB.ReadOnly = True
        tbNamaInGB.ReadOnly = True
        tbIpkInGB.ReadOnly = True

        btnNext.Enabled = True
        btnPrevious.Enabled = True
        btnFirst.Enabled = True
        btnLast.Enabled = True
        btnDelete.Enabled = True
        btnSearch.Enabled = True

        Tampil()
        MessageBox.Show("Data berhasil disimpan.")
    End Sub


    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            fotoBaruPath = Path.GetFileName(OpenFileDialog1.FileName)

            Using imgTemp As Image = Image.FromFile(OpenFileDialog1.FileName)
                PictureBox1.Image = New Bitmap(imgTemp)
            End Using
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim nimDicari As String = tbNIM.Text.Trim()

        If nimDicari = "" Then
            MessageBox.Show("Masukkan NIM yang ingin dicari.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim ditemukan As Boolean = False

        For i As Integer = 0 To DaftarMahasiswa.Count - 1
            If DaftarMahasiswa(i).NIM = nimDicari Then
                indeksAktif = i
                Dim mhs = DaftarMahasiswa(i)

                ' Tampilkan data ke TextBox
                tbNimInGB.Text = mhs.NIM
                tbNamaInGB.Text = mhs.Nama
                tbIpkInGB.Text = mhs.IPK

                ' Tampilkan gambar sesuai folderFoto
                Dim imagePath As String = Path.Combine(folderFoto, mhs.Foto)
                If File.Exists(imagePath) Then
                    Using imgTemp As Image = Image.FromFile(imagePath)
                        PictureBox1.Image = New Bitmap(imgTemp)
                    End Using
                Else
                    PictureBox1.Image = Nothing
                End If

                ditemukan = True
                Exit For
            End If
        Next

        If Not ditemukan Then
            MessageBox.Show("Data dengan NIM tersebut tidak ditemukan.", "Hasil Pencarian", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        modeTambah = True

        ' Kosongkan isian
        tbNimInGB.Text = ""
        tbNamaInGB.Text = ""
        tbIpkInGB.Text = ""
        PictureBox1.Image = Nothing
        fotoBaruPath = ""

        ' Aktifkan input
        tbNimInGB.ReadOnly = False
        tbNamaInGB.ReadOnly = False
        tbIpkInGB.ReadOnly = False
        btnBrowse.Enabled = True
        btnBrowse.Visible = True

        ' Tombol
        btnSave.Enabled = True
        btnSave.Visible = True
        btnEdit.Enabled = False
        btnEdit.Visible = False

        ' Nonaktifkan navigasi
        btnNext.Enabled = False
        btnPrevious.Enabled = False
        btnFirst.Enabled = False
        btnLast.Enabled = False
        btnDelete.Enabled = False
        btnSearch.Enabled = False
    End Sub



    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DaftarMahasiswa.Count = 0 OrElse indeksAktif < 0 OrElse indeksAktif >= DaftarMahasiswa.Count Then
            MessageBox.Show("Tidak ada data yang bisa dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim konfirmasi As DialogResult = MessageBox.Show("Apakah kamu yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If konfirmasi = DialogResult.Yes Then
            ' Hapus data dari list
            DaftarMahasiswa.RemoveAt(indeksAktif)

            ' Simpan kembali ke file .ini
            SimpanKeFileIni(fileIniPath)

            ' Sesuaikan indeks aktif
            If indeksAktif >= DaftarMahasiswa.Count Then
                indeksAktif = DaftarMahasiswa.Count - 1
            End If

            ' Tampilkan data baru atau kosongkan jika tidak ada data
            If DaftarMahasiswa.Count > 0 Then
                Tampil()
            Else
                tbNimInGB.Text = ""
                tbNamaInGB.Text = ""
                tbIpkInGB.Text = ""
                PictureBox1.Image = Nothing
            End If

            MessageBox.Show("Data berhasil dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
