# NX 1919
# Journal created by chenault on Thu Oct 20 12:52:57 2022 GMT Summer Time
#
import math
import NXOpen
import NXOpen.Features
def main() : 

    theSession  = NXOpen.Session.GetSession()
    workPart = theSession.Parts.Work
    displayPart = theSession.Parts.Display
    # ----------------------------------------------
    #   Menu: File->Utilities->Embed Manager...
    # ----------------------------------------------
    markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start")
    
    embedManagerBuilder1 = workPart.Features.CreateEmbedManagerBuilder()
    
    embedManagerBuilder1.Area = NXOpen.Features.EmbedManagerBuilder.UsageAreaTypes.DesignLogic
    
    theSession.SetUndoMarkName(markId1, "Embed Manager Dialog")
    
    theSession.SetUndoMarkVisibility(markId1, None, NXOpen.Session.MarkVisibility.Invisible)
    
    embedManagerBuilder1.ReplaceNativeFileBrowser = "C:\\Work\\Hardpoint_automation\\test_data\\Lukes_data\\P7_FRONT_AXLE_HARDPOINTS_4.1.xlsx"
    
    embeddedfileindex1 = [None] * 1 
    embeddedfileindex1[0] = 0
    embedManagerBuilder1.SetEmbeddedFile(embeddedfileindex1)
    
    embedManagerBuilder1.ReplaceNativeFileBrowser = "C:\\Work\\Hardpoint_automation\\test_data\\Lukes_data\\P7_FRONT_AXLE_HARDPOINTS_4.1.xlsx"
    
    embedManagerBuilder1.Task = NXOpen.Features.EmbedManagerBuilder.TaskTypes.Replace
    
    markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Embed Manager")
    
    nXObject1 = embedManagerBuilder1.Commit()
    
    theSession.DeleteUndoMark(markId2, None)
    
    theSession.SetUndoMarkName(markId1, "Embed Manager")
    
    embedManagerBuilder1.Destroy()
    
    markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start")
    
    embedManagerBuilder2 = workPart.Features.CreateEmbedManagerBuilder()
    
    embedManagerBuilder2.Area = NXOpen.Features.EmbedManagerBuilder.UsageAreaTypes.DesignLogic
    
    theSession.SetUndoMarkName(markId3, "Embed Manager Dialog")
    
    theSession.SetUndoMarkVisibility(markId3, None, NXOpen.Session.MarkVisibility.Invisible)
    
    # ----------------------------------------------
    #   Dialog Begin Embed Manager
    # ----------------------------------------------
    theSession.SetUndoMarkName(markId3, "Embed Manager")
    
    embedManagerBuilder2.Destroy()
    
    # ----------------------------------------------
    #   Menu: Tools->Journal->Stop Recording
    # ----------------------------------------------
    
if __name__ == '__main__':
    main()