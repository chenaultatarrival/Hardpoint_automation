Option Strict Off
Imports System
Imports NXOpen
Imports NXOpen.UF
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
'release notes - v3 updated to update features instead of create duplicates

Module NXJournal
	Public theSession As Session = Session.GetSession()
	Public workPart As Part = theSession.Parts.Work()
	Public lw As ListingWindow = theSession.ListingWindow
	Public theUfSession As UFSession = UFSession.GetUFSession()
	Public ufs As UFSession = UFSession.GetUFSession()
	
	Sub Main(ByVal args() As String)
	
		lw.Open()
		Dim opnt As Point3d
		Dim xpnt As Point3d
		Dim zpnt As Point3d
		Dim newPoint As Point
		Dim oX, oY, oZ As Double
		Dim xX, xY, xZ As Double
		Dim zX, zY, zZ As Double
		Dim Name As String
		Dim EXCEL = CreateObject("Excel.Application")
		EXCEL.Visible = False
		Dim Doc = EXCEL.Workbooks.Open(ReplacementFile, ReadOnly:=True)
		Dim Sheets = EXCEL.Sheets
		Dim Position As String
		Dim oPntName As String
		Dim xPntName As String
		Dim zPntName As String
		Dim CSYSName As String
		Dim thepointcheck As String
		Dim oExists As Integer = 0
		Dim xExists As Integer = 0
		Dim zExists As Integer = 0
		Dim unit1 As Unit =  CType(workpart.UnitCollection.findobject("Millimeter"),Unit)
		Dim Expression1 As Expression
		For i As Integer = 1 To 2
			Dim Sheet = Doc.Sheets.Item(i)
			Dim pointCounter As Integer
			
			If i = 1 Then
				Position = "FRONT" 
			Else If i = 2 Then
				Position = "REAR"
			End If
			
			For pointCounter = 2 To 43
				oExists = 0
				xExists = 0
				zExists = 0
				oX = Sheet.Cells((pointCounter + 1), 5).Value
				
				oY = Sheet.Cells((pointCounter + 1), 6).Value
				oZ = Sheet.Cells((pointCounter + 1), 7).Value
				xX = Sheet.Cells((pointCounter + 1), 8).Value
				xY = Sheet.Cells((pointCounter + 1), 9).Value
				xZ = Sheet.Cells((pointCounter + 1), 10).Value
				zX = Sheet.Cells((pointCounter + 1), 11).Value
				zY = Sheet.Cells((pointCounter + 1), 12).Value
				zZ = Sheet.Cells((pointCounter + 1), 13).Value
				Name = Sheet.Cells((pointCounter + 1), 3).Value
				CSYSName = position.ToUpper & "_" & Name
				opnt = New Point3d(oX, oY, oZ)
				xpnt = New Point3d(xX, xY, xZ)
				zpnt = New Point3d(zX, zY, zZ)

				lw.writeline(Csysname)
				If Name = "" Then 
					lw.Writeline("Empty Cells")
				Else If Name = "TBC"
					lw.writeline("TBC")
				Else
					opnt = New Point3d(oX, oY, oZ)
					lw.writeline(Position & "_" & Name & " = " & oX & ", " & oY & ", " & oZ)				
					lw.writeline(Position & "_" & Name & "_X_ALIGN = " & xX & ", " & xY & ", " & xZ)
					lw.writeline(Position & "_" & Name & "_Z_ALIGN = " & zX & ", " & zY & ", " & zZ)
					oPntName = position & "_ORIGIN_" & Name
					xPntName = position & "_X_ALIGN_" & Name
					zPntName = position & "_Z_ALIGN_" & Name
					
						For Each MyPoint As Point In WorkPart.Points 
							thepointcheck = mypoint.name
							If mypoint.name = oPntName.ToUpper Then
								lw.writeline(oPntName & " Exists")
								oExists = 1
								If mypoint.coordinates.X = oX And mypoint.coordinates.Y = oY And mypoint.coordinates.Z = oZ
									lw.writeline(oPntName & " UNCHANGED")
									mypoint.color = 134
								Else
									myPoint.SetCoordinates(opnt)
									mypoint.SetVisibility(SmartObject.VisibilityOption.Visible)
									mypoint.color = 186
									lw.writeline(oPntName & " Changed (Changes indicated in RED)")
								End If
								Exit For
							Else		
								'lw.writeline(nametobe & " " & mypoint.name & " 	FALSE")
							End If
						Next
						For Each MyPoint As Point In WorkPart.Points
							If mypoint.name = xPntName.ToUpper Then
								lw.writeline(mypoint.name & " " & xPntName & " Exists")
								xExists = 1
								If mypoint.coordinates.X = xX And mypoint.coordinates.Y = xY And mypoint.coordinates.Z = xZ
									lw.writeline(mypoint.name & " " & xPntName & " UNCHANGED")
									mypoint.color = 134
								Else
									myPoint.SetCoordinates(xpnt)
									mypoint.SetVisibility(SmartObject.VisibilityOption.Visible)
									mypoint.color = 186
									lw.writeline(mypoint.name & " " & xPntName & " Changed (Changes indicated in RED)")
								End If
								Exit For
							Else		
								'lw.writeline(nametobe & " " & mypoint.name & " 	FALSE")
							End If
						Next
						For Each MyPoint As Point In WorkPart.Points
							If mypoint.name = zPntName.ToUpper Then
								lw.writeline(zPntName & " Exists")
								zExists = 1
								If mypoint.coordinates.X = zX And mypoint.coordinates.Y = zY And mypoint.coordinates.Z = zZ
									lw.writeline(zPntName & " UNCHANGED")
									mypoint.color = 134
								Else
									myPoint.SetCoordinates(zpnt)
									mypoint.SetVisibility(SmartObject.VisibilityOption.Visible)
									mypoint.color = 186
									lw.writeline(zPntName & " Changed (Changes indicated in RED)")
								End If
								Exit For
							Else		
								'lw.writeline(nametobe & " " & mypoint.name & " 	FALSE")
							End If
						Next
						
						If oExists = 0 Then
							If oX = 0 And oZ = 0 And oY = 0 Then
								lw.writeline(oPntName & " is a 0 value")
							Else
								newPoint = workPart.Points.CreatePoint(opnt)
								newPoint.SetName(oPntName)
								newPoint.Color = 108
								newPoint.SetVisibility(SmartObject.VisibilityOption.Visible)
								lw.writeline(oPntName & " New point created (Changes indicated in GREEN)")
							End If
						End If
						If xExists = 0 Then
							If xX = 0 And xZ = 0 And zY = 0 Then
								lw.writeline(xPntName & " is a 0 value No Point Created")
							Else
								newPoint = workPart.Points.CreatePoint(xpnt)
								newPoint.SetName(xPntName)
								newPoint.Color = 108
								newPoint.SetVisibility(SmartObject.VisibilityOption.Visible)
								lw.writeline(thepointcheck & " " & xPntName & " New point created (Changes indicated in GREEN)")
							End If
						End If
						If zExists = 0 Then
							If zX = 0 And zZ = 0 And zY = 0 Then
								lw.writeline(zPntName & " is a 0 value No Point Created")
							Else
								newPoint = workPart.Points.CreatePoint(zpnt)
								newPoint.SetName(zPntName)
								newPoint.Color = 108
								newPoint.SetVisibility(SmartObject.VisibilityOption.Visible)
								lw.writeline(zPntName & " New point created (Changes indicated in GREEN)")
							End If
						End If
		'datum csys =-=========================================
						Dim Featurecounter As Integer = 0
						Dim recognisedfeatname As String
						Dim recognisedfeat As Features.Feature
						For Each mylookupfeat As Features.Feature In theSession.Parts.Work.Features
							If mylookupfeat.name = CSYSName Then
								Featurecounter = Featurecounter + 1
								recognisedfeatname = mylookupfeat.name
								recognisedfeat = mylookupfeat
								Exit For			
							End If
						Next
						If Featurecounter = 0 Then									
							Try
								CreateDatumCSYS(opnt, xpnt, zpnt, CSYSName)
							Catch ex As Exception
								lw.writeline("Points coincide no csys created @Line: ")
								Exit Try
							End Try
						Else
							EditDatumCsys(opnt, xpnt, zpnt, recognisedfeat)
							lw.writeline(recognisedfeatname & " !! Datum Exists and Updated !!")
						End If
		'datum csys =-=========================================				
						
				End If 
				lw.writeline("@Line: " & Pointcounter)
			lw.writeline(" ------------------------------------------- ")
			Next
		Next
		Doc.Close()
		EXCEL.Quit()
		workPart.ModelingViews.WorkView.Fit()
		'Move to Layer ====================================
				
		Const MovableLayer As Integer = 1
		Dim PointLayer As Integer = 40
		Dim CsysLayer As Integer = 200
		Dim SketchLayer As Integer = 10
		
		For Each MyPoint As Point In WorkPart.Points  
		
			Dim featTag As Tag = Tag.Null
			Dim myFeature As Features.Feature
			ufs.Modl.AskObjectFeat(myPoint.Tag, featTag)
	
			If featTag = Tag.Null Then
			
				'point is unused
				'lw.WriteLine("point is unused")
	
				MyPoint.Layer = PointLayer  
				MyPoint.RedisplayObject  
				
			Else	
			
				myFeature = Utilities.NXObjectManager.Get(featTag)
				'lw.WriteLine("used by: " & myFeature.GetFeatureName)
				Dim FeatName As String = myFeature.GetFeatureName
	
				If FeatName.contains("SKETCH") Then 
					If mypoint.Layer = MovableLayer Then
						myPoint.Layer = SketchLayer  
						myPoint.RedisplayObject  
					End If
	
				ElseIf FeatName.contains("DATUM_CSYS") Then 
					If myPoint.layer = MovableLayer Then
						myPoint.Layer = CsysLayer
						myPoint.RedisplayObject  
					End If
	
				Else 		
					If myPoint.Layer = MovableLayer Then
						myPoint.Layer = PointLayer  
						myPoint.RedisplayObject  
					End If
				End If
	
	
		 	End If
			
		Next 
		
	End Sub
	Public Sub CreateDatumCSYS(ByVal opnt As Point3d, ByVal xpnt As Point3d, ByVal zpnt As Point3d, ByVal CSYSName As String)
        Dim workPart As Part = theSession.Parts.Work
        Dim nullFeatures_Feature As Features.Feature = Nothing
        Dim datumCsysBuilder1 As Features.DatumCsysBuilder
		Dim point1 As Point
		Dim point2 As Point
		Dim point3 As Point
		Dim xform1 As Xform
		Dim cartesianCoordinateSystem1 As CartesianCoordinateSystem
		Dim nXObject1 As NXObject
		Dim Nameing As String
		
        datumCsysBuilder1 = workPart.Features.CreateDatumCsysBuilder(nullFeatures_Feature)
        point1 = workPart.Points.CreatePoint(opnt)      
        point2 = workPart.Points.CreatePoint(xpnt)
        point3 = workPart.Points.CreatePoint(zpnt)
		Nameing = CSYSName
        xform1 = workPart.Xforms.CreateXform(point1, point2, point3, SmartObject.UpdateOption.WithinModeling, 1.0)
        cartesianCoordinateSystem1 = workPart.CoordinateSystems.CreateCoordinateSystem(xform1, SmartObject.UpdateOption.WithinModeling)
        datumCsysBuilder1.Csys = cartesianCoordinateSystem1
        datumCsysBuilder1.DisplayScaleFactor = 1.25
        nXObject1 = datumCsysBuilder1.Commit()
        datumCsysBuilder1.Destroy()
		nXObject1.setname(CSYSName)
    End Sub
	
	Public Sub EditDatumCSYS(ByVal opnt As Point3d, ByVal xpnt As Point3d, ByVal zpnt As Point3d, ByVal recognisedfeat As Features.Feature)
        Dim workPart As Part = theSession.Parts.Work
        Dim nullFeatures_Feature As Features.Feature = Nothing
        Dim datumCsysBuilder1 As Features.DatumCsysBuilder
		Dim point1 As Point
		Dim point2 As Point
		Dim point3 As Point
		Dim xform1 As Xform
		Dim cartesianCoordinateSystem1 As CartesianCoordinateSystem
        datumCsysBuilder1 = workPart.Features.CreateDatumCsysBuilder(recognisedfeat)
        point1 = workPart.Points.CreatePoint(opnt)      
        point2 = workPart.Points.CreatePoint(xpnt)
        point3 = workPart.Points.CreatePoint(zpnt)
        xform1 = workPart.Xforms.CreateXform(point1, point2, point3, SmartObject.UpdateOption.WithinModeling, 1.0)
        cartesianCoordinateSystem1 = workPart.CoordinateSystems.CreateCoordinateSystem(xform1, SmartObject.UpdateOption.WithinModeling)
        datumCsysBuilder1.Csys = cartesianCoordinateSystem1
        datumCsysBuilder1.DisplayScaleFactor = 1.25
        recognisedfeat = datumCsysBuilder1.Commit()
        datumCsysBuilder1.Destroy()
    End Sub
	
	Function ReplacementFile As String
 
        Dim fdlg As OpenFileDialog = New OpenFileDialog()
        fdlg.Title = "Open Point File - (for file format please see @JeffreyBaskett)"
        Dim Dir As String
        Dir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        fdlg.InitialDirectory = Dir
        fdlg.Filter = "Excel Files(*.xls;*.xlsx;*.xlsm)|*.xls;*.xlsx;*.xlsm"
        fdlg.FilterIndex = 2
        fdlg.RestoreDirectory = True
        If fdlg.ShowDialog() = DialogResult.OK Then
            Return fdlg.FileName
        Else
            Return ""
        End If
 
    End Function
End Module