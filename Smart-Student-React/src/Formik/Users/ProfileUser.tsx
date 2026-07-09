import { TextSeparatorSection } from "../../components/TextSeparatorSection";

interface Props {
  imageProfileSelect: string | null;
  setImageProfile: (image: string | null) => void;
  onloadend: (file: File) => void; // Ahora recibe el File
}

export const ProfileUser = ({
  imageProfileSelect,
  setImageProfile,
  onloadend,
}: Props) => {
  return (
    <div className="w-full flex justify-center flex-col items-center mb-2">
      <TextSeparatorSection text="Imagen de Perfil" />
      <div className="mt-2">
        <div className="flex items-center gap-4">
          {/* Contenedor de la imagen de perfil con input oculto */}
          <label
            htmlFor="profileImage"
            className="relative cursor-pointer group"
          >
            {imageProfileSelect ? (
              <div className="relative">
                <img
                  className="rounded-full h-20 w-20 object-cover border-2 border-gray-200 shadow-sm transition duration-300 group-hover:opacity-50"
                  src={imageProfileSelect}
                  alt="avatar"
                />
                {/* Capa de efecto hover */}
                <div className="absolute inset-0 flex items-center justify-center bg-black bg-opacity-40 rounded-full opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth="2"
                    stroke="white"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M12 16l-4-4m0 0l4-4m-4 4h12"
                    />
                  </svg>
                </div>
              </div>
            ) : (
              <div className="rounded-full h-20 w-20 bg-gray-100 border-2 border-gray-200 flex items-center justify-center transition duration-300 group-hover:bg-gray-200">
                <span className="text-gray-400 text-sm group-hover:text-gray-600">
                  Sin imagen
                </span>
              </div>
            )}
            <input
              id="profileImage"
              name="profileImage"
              type="file"
              accept="image/*"
              className="hidden"
              onChange={(event) => {
                if (event.currentTarget.files && event.currentTarget.files[0]) {
                  const file = event.currentTarget.files[0];

                  if (!file.type.startsWith("image/")) {
                    alert("Por favor, selecciona un archivo de imagen.");
                    return;
                  }

                  const reader = new FileReader();
                  reader.onloadend = () => {
                    setImageProfile(reader.result as string);
                    onloadend(file); // Aquí se envía el file al padre
                  };
                  reader.readAsDataURL(file);
                }
              }}
            />
          </label>
        </div>
      </div>
    </div>
  );
};
