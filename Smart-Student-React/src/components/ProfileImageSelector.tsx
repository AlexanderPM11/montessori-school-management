import { useState, useCallback, ChangeEvent } from "react";
import { TextSeparatorSection } from "./TextSeparatorSection";

interface ProfileImageSelectorProps {
  initialImage?: string | null;
  onImageChange: (file: File | null, previewUrl: string | null) => void;
  size?: "sm" | "md" | "lg";
  shape?: "circle" | "square";
  label?: string;
  disabled?: boolean;
}

export const ProfileImageSelector = ({
  initialImage = null,
  onImageChange,
  size = "md",
  shape = "circle",
  label = "Imagen del Centro Educativo",
  disabled = false,
}: ProfileImageSelectorProps) => {
  const [previewUrl, setPreviewUrl] = useState<string | null>(
    initialImage || null
  );

  const sizeClasses = {
    sm: "h-16 w-16",
    md: "h-24 w-24",
    lg: "h-32 w-32",
  };

  const shapeClasses = {
    circle: "rounded-full",
    square: "rounded-lg",
  };

  const handleImageChange = useCallback(
    (event: ChangeEvent<HTMLInputElement>) => {
      if (disabled) return;

      const file = event.target.files?.[0] || null;

      if (!file) {
        onImageChange(null, null);
        setPreviewUrl(null);
        return;
      }

      if (!file.type.startsWith("image/")) {
        alert("Por favor, seleccione un archivo de imagen válido.");
        return;
      }

      const reader = new FileReader();
      reader.onloadend = () => {
        const result = reader.result as string;
        setPreviewUrl(result);
        onImageChange(file, result);
      };
      reader.readAsDataURL(file);
    },
    [onImageChange, disabled]
  );

  const handleRemoveImage = useCallback(() => {
    if (disabled) return;

    setPreviewUrl(null);
    onImageChange(null, null);
  }, [onImageChange, disabled]);

  return (
    <div className="w-full flex flex-col items-center mb-4">
      <TextSeparatorSection text={label} />

      <div className="mt-4 relative group">
        <label
          htmlFor="profileImageInput"
          className={`relative block ${sizeClasses[size]} ${
            shapeClasses[shape]
          } 
            border-2 border-dashed border-gray-300 cursor-pointer 
            overflow-hidden transition-all duration-200
            hover:border-primary-500 hover:shadow-md
            ${disabled ? "opacity-50 cursor-not-allowed" : ""}`}
        >
          {previewUrl ? (
            <img
              src={previewUrl}
              alt="Preview"
              className={`w-full h-full object-cover ${shapeClasses[shape]}`}
            />
          ) : (
            <div
              className={`w-full h-full flex flex-col items-center justify-center 
              bg-gray-100 text-gray-400 ${shapeClasses[shape]}`}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-8 w-8 mb-1"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                />
              </svg>
              <span className="text-xs text-center px-2">
                Seleccionar imagen
              </span>
            </div>
          )}

          {/* Hover overlay */}
          {!disabled && (
            <div
              className={`absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-30 
              flex items-center justify-center transition-all duration-200 ${shapeClasses[shape]}`}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-8 w-8 text-white opacity-0 group-hover:opacity-100 transition-opacity"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M3 9a2 2 0 012-2h.93a2 2 0 001.664-.89l.812-1.22A2 2 0 0110.07 4h3.86a2 2 0 011.664.89l.812 1.22A2 2 0 0018.07 7H19a2 2 0 012 2v9a2 2 0 01-2 2H5a2 2 0 01-2-2V9z"
                />
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M15 13a3 3 0 11-6 0 3 3 0 016 0z"
                />
              </svg>
            </div>
          )}
        </label>

        {/* Remove button (only shown when there's an image) */}
        {previewUrl && !disabled && (
          <button
            type="button"
            onClick={handleRemoveImage}
            className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full 
              h-6 w-6 flex items-center justify-center shadow-md hover:bg-red-600
              transition-colors duration-200 focus:outline-none"
            aria-label="Eliminar imagen"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              className="h-4 w-4"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
        )}

        <input
          id="profileImageInput"
          type="file"
          accept="image/*"
          className="hidden"
          onChange={handleImageChange}
          disabled={disabled}
        />
      </div>

      {!disabled && (
        <p className="mt-2 text-xs text-gray-500 text-center">
          Formatos soportados: JPG, PNG, GIF. Tamaño máximo: 5MB
        </p>
      )}
    </div>
  );
};
