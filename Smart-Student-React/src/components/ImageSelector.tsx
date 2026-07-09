interface Props {
  label?: string;
  image?: string | null;
  onChangeImage: (base64: string | null, file?: File | null) => void;
  aspect?: "square" | "circle";
}

export const ImageSelector = ({
  label = "Seleccionar Imagen",
  image = null,
  onChangeImage,
  aspect = "circle",
}: Props) => {
  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file && file.type.startsWith("image/")) {
      const reader = new FileReader();
      reader.onloadend = () => {
        onChangeImage(reader.result as string, file);
      };
      reader.readAsDataURL(file);
    } else {
      alert("Por favor, selecciona un archivo de imagen válido.");
    }
  };

  const handleRemoveImage = () => {
    onChangeImage(null, null);
  };

  return (
    <div className="flex flex-col items-center space-y-3">
      {label && (
        <span className="text-xl font-medium text-gray-700">{label}</span>
      )}

      <div className="relative group">
        <label htmlFor="image-upload" className="cursor-pointer">
          {image ? (
            <img
              src={image}
              alt="Imagen seleccionada"
              className={`object-cover ${
                aspect === "circle" ? "rounded-full" : "rounded-md"
              } w-32 h-32 border-2 border-gray-300 shadow-md transition-opacity duration-200 group-hover:opacity-60`}
            />
          ) : (
            <div
              className={`flex items-center justify-center bg-gray-100 text-gray-400 ${
                aspect === "circle" ? "rounded-full" : "rounded-md"
              } w-32 h-32 border-2 border-gray-300`}
            >
              Sin imagen
            </div>
          )}
          <input
            id="image-upload"
            type="file"
            accept="image/*"
            className="hidden"
            onChange={handleFileChange}
          />
        </label>

        {image && (
          <button
            type="button"
            onClick={handleRemoveImage}
            className="h-6 w-6 absolute -top-2 -right-2 bg-red-500 text-white rounded-full p-1 text-xs hover:bg-red-600 transition"
            title="Eliminar imagen"
          >
            ✕
          </button>
        )}
      </div>
    </div>
  );
};
