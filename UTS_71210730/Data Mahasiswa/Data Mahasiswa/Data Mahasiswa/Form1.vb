Imports System.IO
Imports System.IO.File  ' 
Public Class Form1
    Dim DataList As New ArrayList()
    Dim indekNim As Integer = 0
    Dim indekNama As Integer = 1
    Dim indekIpk As Integer = 2
    Dim indekFoto As Integer = 3
    Dim DataNim As New ArrayList()

    Private Sub LoadDataFromFile(ByVal filePath As String)
        Dim lines() As String = System.IO.File.ReadAllLines(filePath)
        For Each line As String In lines
            Dim data() As String = line.Split(";" & "\n")
            For Each d As String In data
                DataList.Add(d)
            Next
        Next
        btnNext.Enabled = True
        btnPrevious.Enabled = True
        btnLast.Enabled = True
        btnFirst.Enabled = True
        btnDelete.Enabled = True
        btnSearch.Enabled = True
        btnEdit.Enabled = True
        tbNIM.ReadOnly = False

        Dim a As Integer = 0
        For i = 0 To 4
            DataNim.Add(DataList(a))
            a = a + 4
        Next

    End Sub

    Private Sub btnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click
        btnLoadData.Enabled = False
        LoadDataFromFile("D:\UTS_71210730\UTS_71210730\bahan_uts\data.txt")
        indekNim = 0
        indekNama = 1
        indekIpk = 2
        indekFoto = 3
        Tampil()

    End Sub
    Private Sub Tampil()
        tbNimInGB.Text = DataList(indekNim)
        tbNamaInGB.Text = DataList(indekNama)
        tbIpkInGB.Text = DataList(indekIpk)
        Dim imagePath As String = "D:\UTS_71210730\UTS_71210730\bahan_uts\" & DataList(indekFoto)
        PictureBox1.Image = Image.FromFile(imagePath)

    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If indekNim = 16 Then
            MsgBox("Sudah Mencapai Data Terakhir")
        Else
            indekIpk = indekIpk + 4
            indekNama = indekNama + 4
            indekNim = indekNim + 4
            indekFoto = indekFoto + 4
            Tampil()
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If indekNim = 0 Then
            MsgBox("Sudah Mencapai Data Pertama")
        Else
            indekIpk = indekIpk - 4
            indekNama = indekNama - 4
            indekNim = indekNim - 4
            indekFoto = indekFoto - 4
            Tampil()
        End If
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        indekIpk = 18
        indekNama = 17
        indekNim = 16
        indekFoto = 19
        Tampil()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        indekIpk = 2
        indekNama = 1
        indekNim = 0
        indekFoto = 3
        Tampil()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        btnEdit.Enabled = False
        btnEdit.Visible = False
        btnSave.Enabled = True
        btnSave.Visible = True
        tbIpkInGB.ReadOnly = False
        tbNamaInGB.ReadOnly = False
        tbNimInGB.ReadOnly = False
        DataList(indekNim) = tbNimInGB.Text
        DataList(indekNama) = tbNamaInGB.Text
        DataList(indekIpk) = tbIpkInGB.Text

        btnNext.Enabled = False
        btnPrevious.Enabled = False
        btnLast.Enabled = False
        btnFirst.Enabled = False
        btnDelete.Enabled = False
        btnSearch.Enabled = False
        tbNIM.ReadOnly = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Perbarui data yang di-edit ke DataList
        DataList(indekNim) = tbNimInGB.Text
        DataList(indekNama) = tbNamaInGB.Text
        DataList(indekIpk) = tbIpkInGB.Text

        ' Simpan seluruh DataList ke file
        Dim filePath As String = "D:\UTS_71210730\UTS_71210730\bahan_uts\data.txt"
        Using writer As New StreamWriter(filePath, False)
            For i As Integer = 0 To DataList.Count - 1 Step 4
                Dim line As String = $"{DataList(i)};{DataList(i + 1)};{DataList(i + 2)};{DataList(i + 3)}"
                writer.WriteLine(line)
            Next
        End Using

        ' Kembalikan UI ke mode read-only
        btnEdit.Enabled = True
        btnEdit.Visible = True
        btnSave.Enabled = False
        btnSave.Visible = False
        tbIpkInGB.ReadOnly = True
        tbNamaInGB.ReadOnly = True
        tbNimInGB.ReadOnly = True

        btnNext.Enabled = True
        btnPrevious.Enabled = True
        btnLast.Enabled = True
        btnFirst.Enabled = True
        btnDelete.Enabled = True
        btnSearch.Enabled = True
        tbNIM.ReadOnly = False

        MessageBox.Show("Data berhasil disimpan.")
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Hapus data dari DataList
            DataList.RemoveAt(indekFoto)
            DataList.RemoveAt(indekIpk)
            DataList.RemoveAt(indekNama)
            DataList.RemoveAt(indekNim)

            ' Simpan kembali ke file
            Dim filePath As String = "D:\UTS_71210730\UTS_71210730\bahan_uts\data.txt"
            Using writer As New StreamWriter(filePath, False)
                For i As Integer = 0 To DataList.Count - 1 Step 4
                    Dim line As String = $"{DataList(i)};{DataList(i + 1)};{DataList(i + 2)};{DataList(i + 3)}"
                    writer.WriteLine(line)
                Next
            End Using

            ' Update tampilan
            If indekNim >= DataList.Count Then
                ' Jika data terakhir dihapus, mundur ke data sebelumnya
                indekNim -= 4 : indekNama -= 4 : indekIpk -= 4 : indekFoto -= 4
            End If

            If DataList.Count > 0 Then
                Tampil()
            Else
                tbNimInGB.Clear()
                tbNamaInGB.Clear()
                tbIpkInGB.Clear()
                PictureBox1.Image = Nothing
                MessageBox.Show("Semua data telah dihapus.")
            End If

        ElseIf result = DialogResult.No Then
            MessageBox.Show("Data tidak jadi dihapus.")
        End If
    End Sub

End Class