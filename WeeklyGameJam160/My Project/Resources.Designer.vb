'------------------------------------------------------------------------------
' <auto-generated>
'     Ce code a été généré par un outil.
'     Version du runtime :4.0.30319.42000
'
'     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
'     le code est régénéré.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    'à l'aide d'un outil, tel que ResGen ou Visual Studio.
    'Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    'avec l'option /str ou régénérez votre projet VS.
    '''<summary>
    '''  Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("WeeklyGameJam160.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Remplace la propriété CurrentUICulture du thread actuel pour toutes
        '''  les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property background() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("background", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property color_frame() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("color_frame", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property end_of_game() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("end_of_game", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property fade() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("fade", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Byte[].
        '''</summary>
        Friend ReadOnly Property font() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("font", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Icon semblable à (Icône).
        '''</summary>
        Friend ReadOnly Property icon() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("icon", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property level_complete() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level_complete", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''
        '''</summary>
        Friend ReadOnly Property levels() As String
            Get
                Return ResourceManager.GetString("levels", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property menu() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("menu", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_button() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_button", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_completeend() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_completeend", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_eog() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_eog", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_eol() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_eol", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_music() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_music", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.IO.UnmanagedMemoryStream semblable à System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_tile() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_tile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property tiles() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("tiles", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Recherche une ressource localisée de type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property wall() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("wall", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
