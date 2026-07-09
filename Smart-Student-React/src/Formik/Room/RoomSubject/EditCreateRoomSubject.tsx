import { useEffect, useState } from "react";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../../util/showCustomLoading";
import { FormFields } from "./FormFields";
import { validationSchema } from "./registrationSchema";
import {
  Formik,
  ManageLocalStorage,
  ToastifyCustom,
  useGeneralStore,
  useNavigate,
  useSearchParams,
} from "../../Users/user";
import { Form } from "react-router-dom";
import { roomSubjectStore } from "../../../hooks/store/Room/RoomSubject";
import { RoomSubject } from "../../../interfaces/Room/RoomSubject";
import { useSubjectStore } from "../../../hooks/store/Subject/Subject.store";
import { BtnsForm } from "./BtnsForm";

export const EditCreateroomSubject = () => {
  const [isLoading, setIsLoading] = useState(true);

  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const idRoomSubject = Number(searchParams.get("id-subject-room"));
  const idRoom = Number(searchParams.get("idRoom"));
  const isEdit = searchParams.get("isEdit") === "true";

  const onCreateOrUpdate = roomSubjectStore((state) => state.onCreateOrUpdate);
  const roomSubjects = roomSubjectStore((state) => state.roomSubjet);

  const getTeachersByIdRoom = useGeneralStore(
    (state) => state.getTeachersByIdRoom
  );
  const teachers = useGeneralStore((state) => state.teachersByRoom);

  const getSubject = useSubjectStore((state) => state.getSubject);
  const subjects = useSubjectStore((state) => state.subjects);

  let subjectData: RoomSubject | null = null;
  if (!subjectData && isEdit) {
    subjectData =
      ManageLocalStorage.readFromLocalStorage<RoomSubject>(
        "roomTeacherEditing"
      );
  }
  if (isEdit && idRoomSubject && roomSubjects.length > 0) {
    subjectData = roomSubjects.find((x) => x.id === idRoomSubject) || null;

    if (subjectData) {
      ManageLocalStorage.saveToLocalStorage("roomTeacherEditing", subjectData);
    }
  }
  useEffect(() => {
    const init = async () => {
      showCustomLoading();
      await getTeachersByIdRoom(subjectData?.idRoom || idRoom || 0);
      await getSubject();

      setIsLoading(false);
      closeCustomLoading();
    };

    init();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const initialValues = {
    id: subjectData?.id || 0,
    idTeacher: subjectData?.idTeacher || "",
    idSuject: subjectData?.idSuject?.toString() || "",
    idRoom: subjectData?.idRoom || idRoom || 0,
  };

  const handleUserSubmit = async (values: typeof initialValues) => {
    const subject = subjects.find((s) => s.id === Number(values.idSuject));
    const teacher = teachers.find((t) => t.id === values.idTeacher);

    const newData: RoomSubject = {
      id: isEdit ? idRoomSubject : 0,
      idTeacher: values.idTeacher,
      idSuject: Number(values.idSuject),
      idRoom: values.idRoom,
      nameTeacher: teacher?.text || "",
      nameGrade: subject?.name || "",
    };

    const finalData = isEdit
      ? { ...(subjectData as RoomSubject), ...newData }
      : newData;

    showCustomLoading();
    const result = await onCreateOrUpdate(finalData);
    closeCustomLoading();

    ToastifyCustom({
      options: { autoClose: 2000 },
      message:
        result.message ||
        (result.result
          ? isEdit
            ? "Asignación actualizada correctamente"
            : "Asignación creada correctamente"
          : "Ocurrió un error"),
      type: result.result ? "success" : "error",
    });

    if (result.result) navigate(-1);
  };

  return (
    <div className="mt-5 md:mb-16">
      {!isLoading && (
        <>
          <h2 className="text-lg sm:text-xl md:text-2xl font-semibold mb-6 text-gray-800">
            {isEdit
              ? "Edición de vínculo materia - docente"
              : "Nueva asignación de materia a docente"}
          </h2>

          <Formik
            initialValues={initialValues}
            validationSchema={validationSchema}
            onSubmit={handleUserSubmit}
            enableReinitialize
          >
            {({ handleSubmit }) => (
              <Form onSubmit={handleSubmit} noValidate>
                <FormFields />
                <BtnsForm />
              </Form>
            )}
          </Formik>
        </>
      )}
    </div>
  );
};
