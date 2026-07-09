import { TextSeparatorSection } from "../../../components/TextSeparatorSection";
import { formatCardID, formatTel } from "../../../util/generalFormattedField";
import { CustomInput } from "../../CustomInput";
import { CustomMultiSelect } from "../../CustomMultiSelect";
import { useGeneralStore } from "../user";
export const FormFields = () => {
  const civilStatus = useGeneralStore((state) => state.civilStatus);
  const genders = useGeneralStore((state) => state.genders);
  const nationality = useGeneralStore((state) => state.nationality);
  const educationalLevel = useGeneralStore((state) => state.educationalLevel);
  const relationship = useGeneralStore((state) => state.relationship);
  const professions = useGeneralStore((state) => state.professions);

  return (
    <div className="flex flex-wrap gap-4">
      <div className="w-full">
        <TextSeparatorSection text="Información Personal" />
      </div>
      {/* Primera columna */}
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="name"
          placeholder="Nombre(s):"
          type="text"
          label="Ingrese el nombre:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="lastname"
          placeholder="Ingrese los apellidos"
          type="text"
          label="Apellidos:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="userName"
          placeholder="Ingrese el nombre de usuario"
          type="text"
          label="Nombre de Usuario:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="gender"
          label="Género:"
          options={genders}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="dateBirth"
          placeholder="Fecha de Nacimiento"
          type="date"
          label="Fecha de Nacimiento:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="cardId"
          placeholder="Cédula de identidad"
          type="text"
          label="Ingrese la cédula de identidad:"
          onChangeCustom={(value, previousValue) => {
            const formattedValue = formatCardID(value, previousValue);
            return formattedValue;
          }}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="noBook"
          placeholder="No. Libro"
          type="text"
          label="No. Libro:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="noFolio"
          placeholder="No. Folio"
          type="text"
          label="No. Folio:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="civilStatus"
          label="Estado Civil:"
          options={civilStatus}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="nationality"
          label="Nacionalidad:"
          options={nationality}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="educationLevel"
          label="Nivel educativo:"
          options={educationalLevel}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          name="relationship"
          label="Parentesco:"
          options={relationship}
        />
      </div>
      {/* <div className="w-full">
        <CustomMultiSelect
          requerid={true}
          name="roles"
          label="Roles:"
          isMulti={true}
          options={rolesOptions}
        />
      </div> */}

      <div className="w-full">
        <TextSeparatorSection text="Contacto" />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="email"
          placeholder="Ingrese el correo"
          type="email"
          label="Correo:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="tel"
          placeholder="Ingrese el número de teléfono:"
          type="text"
          label="Número de Teléfono:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="phone"
          placeholder="Ingrese el número de celular:"
          type="tel"
          label="Celular:"
          onChangeCustom={(value, previousValue) => {
            const formattedValue = formatTel(value, previousValue);
            return formattedValue;
          }}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="addres"
          placeholder="Ingrese la dirección"
          type="text"
          label="Dirección:"
        />
      </div>
      <div className="w-full">
        <TextSeparatorSection text="Ocupación y Trabajo" />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          name="profession"
          label="Profesión:"
          options={professions}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect name="job" label="Trabajo:" options={professions} />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          name="placeJob"
          placeholder="Lugar de Trabajo"
          type="text"
          label="Lugar de Trabajo:"
        />
      </div>
    </div>
  );
};
