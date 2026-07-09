import { CustomInput } from "../CustomInput";

export const FormFields = () => {
  return (
    <div className="flex flex-wrap gap-4">
      {/* Primera columna */}
      <div className="w-full md:w-[calc(100%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="name"
          placeholder="Nombre:"
          type="text"
          label="Ingrese el nombre:"
        />
      </div>

      <div className="w-full md:w-[calc(100%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="description"
          placeholder="Descripción"
          type="textarea"
          label="Descripción:"
        />
      </div>
    </div>
  );
};
