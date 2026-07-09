import { TextSeparatorSection } from "../../components/TextSeparatorSection";
import { useGeneralStore } from "../../hooks/store/General/General.store";
import { formatTel } from "../../util/generalFormattedField";
import { CustomInput } from "../CustomInput";
import { CustomMultiSelect } from "../CustomMultiSelect";
import { useUserStore } from "../Users/user";

export const FormFields = () => {
  const genders = useGeneralStore((state) => state.genders);
  const nationality = useGeneralStore((state) => state.nationality);
  const grades = useGeneralStore((state) => state.grades);
  const relationship = useGeneralStore((state) => state.relationship);
  const levels = useGeneralStore((state) => state.levels);
  const fathersAndMothers = useUserStore((state) => state.fathersAndMothers);

  const carriedPreprimary = [
    { id: false, text: "No" },
    { id: true, text: "Sí" },
  ];
  // useEffect(() => {}, []);

  return (
    <div className="flex flex-wrap gap-4">
      <div className="w-full">
        <TextSeparatorSection text="Datos Personales" />
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
        <CustomMultiSelect
          requerid={true}
          name="sexo"
          label="Sexo:"
          options={genders}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          name="relationship"
          label="Parentesco:"
          options={relationship}
          requerid={true}
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
      <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col md:flex-row gap-4 justify-between">
        <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col gap-2">
          <CustomInput
            name="noFolio"
            placeholder="No. Folio"
            type="text"
            label="No. Folio:"
          />
        </div>
        <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col gap-2">
          <CustomMultiSelect
            requerid={true}
            name="idGrade"
            label="Grado:"
            options={grades}
          />
        </div>
      </div>

      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idNacionality"
          label="Nacionalidad:"
          options={nationality}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="bornDate"
          placeholder="Fecha de Nacimiento"
          type="date"
          label="Fecha de Nacimiento:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="direction"
          placeholder="Ingrese la dirección"
          type="text"
          label="Dirección:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col md:flex-row gap-4 justify-between">
        <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col gap-2">
          <CustomMultiSelect
            requerid={true}
            name="idTypeRegister"
            label="Nivel:"
            options={levels}
          />
        </div>
        <div className="w-full md:w-[calc(50%-0.5rem)] flex flex-col gap-2">
          <CustomMultiSelect
            requerid={true}
            name="carriedPreprimary"
            label="¿Realizó el Preprimario?"
            options={carriedPreprimary}
          />
        </div>
      </div>

      <div className="w-full">
        <TextSeparatorSection text="Datos de los Hermanos" />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="numberSiblings"
          placeholder="No. de hermanos"
          type="text"
          label="No. de hermanos:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="placeBetweenSiblings"
          placeholder="Lugar entre hermanos"
          type="text"
          label="Lugar entre hermanos:"
        />
      </div>
      <div className="w-full">
        <TextSeparatorSection text="Datos Médicos" />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="neae"
          placeholder="(NEAE)"
          type="textarea"
          label="Necesidades Específicas de Apoyo Educativo (NEAE):"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="diseasesAllergic"
          placeholder="Alergias o enfermedades"
          type="textarea"
          label="Alergias o enfermedades:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="medicinesUse"
          placeholder="Medicamentos que utiliza"
          type="textarea"
          label="Medicamentos que utiliza:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={false}
          name="doctorPediatrician"
          placeholder="Médico o Pediatra"
          type="text"
          label="Médico o Pediatra:"
        />
      </div>
      <div className="w-full">
        <TextSeparatorSection text="Padres o Tutores" />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idFather"
          label="Padre/Tutor"
          options={fathersAndMothers.fathers}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idMother"
          label="Madre/Tutor"
          options={fathersAndMothers.mothers}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="telFather"
          placeholder="Teléfono de Padre/Tutor"
          type="tel"
          label="Teléfono de Padre/Tutor:"
          onChangeCustom={(value, previousValue) => {
            const formattedValue = formatTel(value, previousValue);
            return formattedValue;
          }}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="telMother"
          placeholder="Teléfono de Madre/Tutor"
          type="tel"
          label="Teléfono de Madre/Tutor:"
          onChangeCustom={(value, previousValue) => {
            const formattedValue = formatTel(value, previousValue);
            return formattedValue;
          }}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="emergencyPerson"
          placeholder="Persona para llamar de emergencia"
          type="text"
          label="Persona para llamar de emergencia:"
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomInput
          requerid={true}
          name="emergencyTel"
          placeholder="Teléfono de emergencia"
          type="tel"
          label="Teléfono de emergencia:"
          onChangeCustom={(value, previousValue) => {
            const formattedValue = formatTel(value, previousValue);
            return formattedValue;
          }}
        />
      </div>
    </div>
  );
};
