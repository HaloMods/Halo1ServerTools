Imports System.Drawing

Namespace ScreenUtils
    Public Class ScreenCapture

        ' The ScreenCapture class allows you to take screenshots (printscreens)
        ' of the desktop or of individual windows.
        '
        ' Usage:
        '
        ' PictureBox1.Image = ScreenCapture.GrabScreen()
        ' PictureBox1.Image = ScreenCapture.GrabActiveWindow()
        ' PictureBox1.Image = ScreenCapture.GrabWindow(SomeHwnd)
        '
        ' PictureBox1.Image = ScreenCapture.GrabScreen(X, Y, Width, Height)
        ' PictureBox1.Image = ScreenCapture.GrabScreen(Rect)
        ' PictureBox1.Image = ScreenCapture.GrabScreen(Location, Size)


#Region "Constants"

        Private Const HORZRES As Integer = 8
        Private Const VERTRES As Integer = 10
        Private Const SRCCOPY = &HCC0020
        Private Const SRCINVERT = &H660046

        Private Const USE_SCREEN_WIDTH = -1
        Private Const USE_SCREEN_HEIGHT = -1

#End Region

#Region "API's"

        Private Structure RECT
            Public Left As Int32
            Public Top As Int32
            Public Right As Int32
            Public Bottom As Int32
        End Structure

        Private Declare Function CreateDC Lib "gdi32" Alias "CreateDCA" (ByVal lpDriverName As String, ByVal lpDeviceName As String, ByVal lpOutput As String, ByVal lpInitData As String) As Integer
        Private Declare Function CreateCompatibleDC Lib "GDI32" (ByVal hDC As Integer) As Integer
        Private Declare Function DeleteDC Lib "GDI32" (ByVal hDC As Integer) As Integer
        Private Declare Function GetWindowDC Lib "user32" Alias "GetWindowDC" (ByVal hwnd As Long) As Integer
        Private Declare Function ReleaseDC Lib "user32" Alias "ReleaseDC" (ByVal hwnd As Long, ByVal hdc As Long) As Long
        Private Declare Function GetDeviceCaps Lib "gdi32" Alias "GetDeviceCaps" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer
        Private Declare Function CreateCompatibleBitmap Lib "GDI32" (ByVal hDC As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer) As Integer
        Private Declare Function SelectObject Lib "GDI32" (ByVal hDC As Integer, ByVal hObject As Integer) As Integer
        Private Declare Function DeleteObject Lib "GDI32" (ByVal hObj As Integer) As Integer
        Private Declare Function BitBlt Lib "GDI32" (ByVal hDestDC As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal SrcX As Integer, ByVal SrcY As Integer, ByVal Rop As Integer) As Integer
        Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer
        Private Declare Function IsWindow Lib "user32" Alias "IsWindow" (ByVal hwnd As Integer) As Long
        Private Declare Function GetWindowRect Lib "user32.dll" (ByVal hWnd As Integer, ByRef lpRect As RECT) As Int32

#End Region

#Region "Public Methods."

        Public Shared Function GrabScreen() As Bitmap

            Return GrabScreen(0, 0, USE_SCREEN_WIDTH, USE_SCREEN_HEIGHT)

        End Function

        Public Shared Function GrabScreen(ByVal Rect As Rectangle) As Bitmap

            Return GrabScreen(Rect.X, Rect.Y, Rect.Width, Rect.Height)

        End Function

        Public Shared Function GrabScreen(ByVal Location As System.Drawing.Point, ByVal Size As System.Drawing.Size) As Bitmap

            Return GrabScreen(Location.X, Location.Y, Size.Width, Size.Height)

        End Function

        Public Shared Function GrabScreen(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer) As Bitmap

            Dim hDesktopDC As Integer
            Dim hOffscreenDC As Integer
            Dim hBitmap As Integer
            Dim hOldBmp As Integer
            Dim MyBitmap As Bitmap

            ' Get the desktop device context.
            hDesktopDC = CreateDC("DISPLAY", "", "", "")
            If hDesktopDC Then
                ' Adjust width and height.
                If Width = USE_SCREEN_WIDTH Then
                    Width = GetDeviceCaps(hDesktopDC, HORZRES)
                End If
                If Height = USE_SCREEN_HEIGHT Then
                    Height = GetDeviceCaps(hDesktopDC, VERTRES)
                End If
                ' Create an offscreen device context.
                hOffscreenDC = CreateCompatibleDC(hDesktopDC)
                If hOffscreenDC Then
                    ' Create a bitmap for our offscreen device context.
                    hBitmap = CreateCompatibleBitmap(hDesktopDC, Width, Height)
                    If hBitmap Then
                        ' Copy the image and create an instance of the Bitmap class.
                        hOldBmp = SelectObject(hOffscreenDC, hBitmap)
                        BitBlt(hOffscreenDC, 0, 0, Width, Height, hDesktopDC, X, Y, SRCCOPY)
                        MyBitmap = Bitmap.FromHbitmap(New IntPtr(hBitmap))
                        ' Clean up.
                        DeleteObject(SelectObject(hOffscreenDC, hOldBmp))
                    End If
                    DeleteDC(hOffscreenDC)
                End If
                DeleteDC(hDesktopDC)
            End If
            ' Return our Bitmap instance.
            Return MyBitmap

        End Function

        Public Shared Function GrabActiveWindow() As Bitmap

            Return GrabWindow(GetForegroundWindow())

        End Function

        Public Shared Function GrabWindow(ByVal hWnd As Int32) As Bitmap

            Dim hWindowDC As Long
            Dim hOffscreenDC As Long
            Dim rec As RECT
            Dim nWidth As Long
            Dim nHeight As Long
            Dim hBitmap As Long
            Dim hOldBmp As Long
            Dim MyBitmap As Bitmap

            ' Verify if a valid window handle was provided.
            If hWnd <> 0 And IsWindow(hWnd) Then
                ' Get the window's device context.
                hWindowDC = GetWindowDC(hWnd)
                If hWindowDC Then
                    ' Get width and height.
                    If GetWindowRect(hWnd, rec) Then
                        nWidth = rec.Right - rec.Left
                        nHeight = rec.Bottom - rec.Top
                        ' Create an offscreen device context.
                        hOffscreenDC = CreateCompatibleDC(hWindowDC)
                        If hOffscreenDC Then
                            ' Create a bitmap for our offscreen device context.
                            hBitmap = CreateCompatibleBitmap(hWindowDC, nWidth, nHeight)
                            If hBitmap Then
                                ' Copy the image and create an instance of the Bitmap class.
                                hOldBmp = SelectObject(hOffscreenDC, hBitmap)
                                BitBlt(hOffscreenDC, 0, 0, nWidth, nHeight, hWindowDC, 0, 0, SRCCOPY)
                                MyBitmap = Bitmap.FromHbitmap(New IntPtr(hBitmap))
                                ' Clean up.
                                DeleteObject(SelectObject(hOffscreenDC, hOldBmp))
                            End If
                            DeleteDC(hOffscreenDC)
                        End If
                    End If
                    ReleaseDC(hWnd, hWindowDC)
                End If
            End If
            ' Return our Bitmap instance.
            Return MyBitmap

        End Function

#End Region

    End Class
End Namespace