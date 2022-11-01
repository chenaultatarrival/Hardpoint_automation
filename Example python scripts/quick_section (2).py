#nx: threaded
import sys
import typing as tp
import NXOpen
import NXOpen.UF
import NXOpen.Assemblies


POINT_NAME = "DatumPoint"
SECTION_NAME = "JointSection"


def create_section(position: tp.List[float], datum_axis: tp.List[float]):
    session = NXOpen.Session.GetSession()
    uf_session = NXOpen.UF.UFSession.GetUFSession()
    root = session.Parts.Work
    datum_position = NXOpen.Point3d()
    datum_position.X = position[0]
    datum_position.Y = position[1]
    datum_position.Z = position[2]

    ob_w = uf_session.Vec3.Negate(datum_axis)
    if abs(abs(ob_w[1]) - 1.0) < 0.001:
        ob_u = [0.0, 0.0, 1.0 if ob_w[1] < 0 else -1.0]
    else:
        ob_u = [ob_w[2], 0.0, -ob_w[0]]

    section_normal = ob_u

    is_new_point = True
    point_feature = NXOpen.Features.Feature.Null
    for feature in root.Features:
        if feature.Name == POINT_NAME:
            is_new_point = False
            point_feature = feature
            break

    point_feature_builder = root.BaseFeatures.CreatePointFeatureBuilder(point_feature)
    if is_new_point:
        datum_point = root.Points.CreatePoint(datum_position)
        datum_point.SetName(POINT_NAME)
        datum_point.SetVisibility(NXOpen.SmartObject.VisibilityOption.Visible)

        point_feature_builder.Point = datum_point
        point_feature_builder.Commit()
        feature = point_feature_builder.GetFeature()
        feature.SetName(POINT_NAME)
    else:
        point_feature_builder.Point.SetCoordinates(datum_position)
        point_feature_builder.Commit()

    point_feature_builder.Destroy()


    existing_section = NXOpen.Display.DynamicSection.Null
    is_new = True
    for section in root.DynamicSections:
        if section.Name == SECTION_NAME:
            is_new = False
            existing_section = section
            break

    section_builder = root.DynamicSections.CreateSectionBuilder(existing_section,
                                                                session.Parts.Work.ModelingViews.WorkView)
    section_builder.SetOrigin(datum_position)

    section_builder.SetNormal(NXOpen.Vector3d(section_normal[0], section_normal[1], section_normal[2]))
    if is_new:
        section_builder.SetName(SECTION_NAME)
        section_builder.CapColorOption = NXOpen.Display.DynamicSectionTypesCapColorOption.Body
        section_builder.ShowClip = True
        section_builder.CsysType = NXOpen.Display.DynamicSectionTypes.CoordinateSystem.Absolute

    section = section_builder.Commit()

    if section:
        view_normal = ob_u
        view_back = view_normal
        view_right = uf_session.Vec3.Cross(ob_w, view_back)
        view_up = uf_session.Vec3.Cross(view_back, view_right)

        mat = uf_session.Mtx3.Initialize(view_right, view_up)
        mat2 = NXOpen.Matrix3x3(mat[0], mat[1], mat[2], mat[3], mat[4], mat[5], mat[6], mat[7], mat[8])

        view_origin = NXOpen.Point3d()
        origin = [-position[0], -position[1], -position[2]]
        pt = uf_session.Mtx3.VecMultiply(origin, mat)
        view_origin.X = pt[0]
        view_origin.Y = pt[1]
        view_origin.Z = pt[2]

        session.Parts.Work.ModelingViews.WorkView.SetRotationTranslationScale(
            mat2, view_origin, 1.0)

        section_builder.Destroy()


def main(argv):
    x = argv[0]
    y = argv[1]
    z = argv[2]
    position = [float(x), float(y), float(z)]
    if len(argv) == 6:
        ax = argv[3]
        ay = argv[4]
        az = argv[5]
        datum_axis = [float(ax), float(ay), float(az)]
    else:
        datum_axis = [0.0, 0.0, 1.0]

    create_section(position, datum_axis)


if __name__ == '__main__':
    main(sys.argv[1:])
