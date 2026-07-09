import { FileUploader } from "../../components/FileUploader";
import { useRoomsStore } from "../../hooks/store/Room/Rooms.store";
import { Room } from "../../interfaces/Room/Room";
import {
  closeCustomLoading,
  showCustomLoading,
} from "../../util/showCustomLoading";
import {
  Formik,
  Form,
  useNavigate,
  useSearchParams,
  useGeneralStore,
  ManageLocalStorage,
  useState,
  useEffect,
  ToastifyCustom,
  BtnsForm,
} from "../Users/user";
import { FormFields } from "./FormFields";
import { validationSchema } from "./registrationSchema";

export const EditCreateRoomForm = () => {
  const [base64, setBase64] = useState<string | null>(null);

  const onCreateOrUpdate = useRoomsStore((state) => state.onCreateOrUpdate);
  const getLevels = useGeneralStore((state) => state.getLevels);
  const getTeacher = useGeneralStore((state) => state.getTeacher);
  const setIsLoading = useGeneralStore((state) => state.setIsLoading);
  const navigate = useNavigate();

  const loading = useGeneralStore((state) => state.loading);
  const levels = useGeneralStore((state) => state.levels);
  const [searchParams] = useSearchParams();
  const rooms = useRoomsStore((state) => state.rooms);
  const idRoom = Number(searchParams.get("idRoom"));

  let RoomSelect: Room | undefined | null;

  if (idRoom) {
    if (rooms.length > 0) {
      RoomSelect = rooms.find((x) => x.id === idRoom);
    }
    if (RoomSelect) {
      ManageLocalStorage.saveToLocalStorage("roommEditing", RoomSelect);
    } else {
      RoomSelect =
        ManageLocalStorage.readFromLocalStorage<Room>("roommEditing");
    }
  }

  const [imageFile, setImageFile] = useState<File | null>(null);

  const getData = async () => {
    setIsLoading(true);
    await getLevels();
    await getTeacher();
    setIsLoading(false);
  };

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
    getData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // User Info
  const roomInfo = {
    id: RoomSelect?.id || 0,
    name: RoomSelect?.name || "",
    description: RoomSelect?.description || "",
    addres: RoomSelect?.location || "",
    capacity: RoomSelect?.capacity || 0,
    level: RoomSelect?.idTypeRegistersList || [],
    idTeacherLead: RoomSelect?.idTeacherLead || "",
  };

  const handleUserSubmit = async (values: typeof roomInfo) => {
    const levelIds = values.level
      .toString()
      .split(",")
      .map((id) => parseInt(id.trim(), 10));
    const matchingItems = levels.filter((item) => levelIds.includes(item.id));
    const namesString = matchingItems.map((item) => item.text).join(", ");
    const userToSave = {
      id: Number(roomInfo.id) || 0,
      name: values?.name || "",
      description: values?.description || "",
      location: values?.addres || "",
      capacity: values?.capacity || 0,
      idTeacherLead: values?.idTeacherLead || "",
      level: values?.level.join(",") || "",
      file: imageFile ?? undefined,
      levelBack: namesString,
      idTypeRegistersBack: values.level.join(",") || "",
      imageUrl: base64 || RoomSelect?.imageUrl || "",
    };
    const mergedRoom = {
      ...(RoomSelect as Room),
      ...userToSave,
    };
    showCustomLoading();
    const result = await onCreateOrUpdate(mergedRoom);
    console.log(result);
    closeCustomLoading();

    if (result.result) {
      ToastifyCustom({
        options: { autoClose: 2000 },
        message: result.message ?? "Datos actualizados exitosamente",
        type: "success",
      });
      navigate("/rooms", { replace: true });
    } else {
      ToastifyCustom({
        message: result.message ?? "An error occurred",
        type: "error",
      });
    }
  };

  if (loading) {
    showCustomLoading();
  } else {
    closeCustomLoading();
  }

  return (
    <div className="mt-5 md:mb-16">
      {!loading && (
        <div>
          <Formik
            initialValues={roomInfo}
            validationSchema={validationSchema}
            onSubmit={handleUserSubmit}
            enableReinitialize
          >
            {({ handleSubmit }) => (
              <div>
                <Form onSubmit={handleSubmit} noValidate>
                  {/* Campos del formulario */}
                  <FormFields />
                  <div className="mt-4">
                    <FileUploader
                      accept="image/*"
                      title="Imagen"
                      onFileSelect={(fileLoad, base) => {
                        if (fileLoad) {
                          setImageFile(fileLoad);
                          setBase64(base);
                        }
                      }}
                      initialBase64={RoomSelect?.imageUrl}
                    />
                  </div>
                  {/* Botones al final del formulario */}
                  <BtnsForm navigateLink="/rooms" />
                </Form>
              </div>
            )}
          </Formik>
        </div>
      )}
    </div>
  );
};
