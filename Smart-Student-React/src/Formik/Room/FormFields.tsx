import { useGeneralStore } from "../../hooks/store/General/General.store";
import { CustomInput } from "../CustomInput";
import { CustomMultiSelect } from "../CustomMultiSelect";

export const FormFields = () => {
  const levels = useGeneralStore((state) => state.levels);
  const teachers = useGeneralStore((state) => state.teachers);

  return (
    <div className="flex flex-wrap gap-4">
      {/* Primera columna */}
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="name"
          placeholder="Nombre:"
          type="text"
          label="Ingrese el nombre:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="description"
          placeholder="Ingrese los descripción"
          type="textarea"
          rows={1}
          label="Descripción:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idTeacherLead"
          label="Profesor Encargado:"
          options={teachers}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="addres"
          placeholder="Ubicación"
          type="text"
          label="Ubicación:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="capacity"
          placeholder="Capacidad"
          type="text"
          label="Capacidad:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="level"
          label="Niveles:"
          isMulti={true}
          options={levels}
        />
      </div>
    </div>
  );
};
