import { CustomInput } from "../CustomInput";
import { CustomMultiSelect } from "../CustomMultiSelect";
import { useGeneralStore } from "../../hooks/store/General/General.store";

export const FormFields = () => {
  const sessions = [
    { id: "Matutina", text: "Matutina" },
    { id: "Vespertina", text: "Vespertina" },
    { id: "Nocturna", text: "Nocturna" },
  ];
  const generalUserRoles = useGeneralStore((state) => state.generalUserRoles);
  const provices = useGeneralStore((state) => state.provices);

  return (
    <div className="flex flex-wrap gap-4">
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="name"
          label="Nombre"
          placeholder="Ingrese el nombre del centro educativo"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="address"
          label="Dirección"
          placeholder="Ingrese la dirección del centro"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid
          name="idProvinceDom"
          label="Provincia"
          options={provices}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="nameMunicipality"
          label="Municipio"
          placeholder="Ingrese el municipio"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="phone"
          label="Teléfono"
          placeholder="Ej: 809-555-1234"
          type="tel"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="mobile"
          label="Móvil"
          placeholder="Ej: 809-555-5678"
          type="tel"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid
          name="idRector"
          label="Seleccionar Rector"
          options={generalUserRoles.Rector}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid
          name="idCordinator"
          label="Seleccionar Coordinador"
          options={generalUserRoles.Cordinador}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          name="idSecretary"
          label="Seleccionar Secretario/a"
          options={generalUserRoles.Secretario}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          name="idDepartment"
          label="Departamento"
          options={[]}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid
          name="session"
          label="Tanda"
          options={sessions}
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="regional"
          label="Regional"
          placeholder="Ej: Regional 01"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="educationalRegistry"
          label="Código del Centro"
          placeholder="Ej: #12345"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid
          name="district"
          label="Distrito"
          placeholder="Ej: Distrito 03"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="website"
          label="Sitio Web"
          placeholder="Ej: www.ejemplo.edu.do"
          type="text"
        />
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="academicResolution"
          label="Resolución Académica"
          placeholder="Ingrese la resolución académica"
          type="textarea"
        />
      </div>
    </div>
  );
};
