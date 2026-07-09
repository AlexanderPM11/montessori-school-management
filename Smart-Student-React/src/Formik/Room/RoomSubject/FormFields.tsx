import { useSubjectStore } from "../../../hooks/store/Subject/Subject.store";
import { CustomMultiSelect } from "../../CustomMultiSelect";
import { useGeneralStore } from "../../Users/user";

export const FormFields = () => {
  const subjects = useSubjectStore((state) => state.subjects);
  const subjectMaps = subjects.map((subject) => ({
    id: subject.id,
    text: subject.name,
  }));

  const teachers = useGeneralStore((state) => state.teachersByRoom);

  return (
    <div className="flex flex-wrap gap-4">
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idTeacher"
          label="Profesor:"
          options={teachers}
        />
      </div>
      <div className="w-full md:w-[calc(50%-0.5rem)]">
        <CustomMultiSelect
          requerid={true}
          name="idSuject"
          label="Asignatura:"
          options={subjectMaps}
        />
      </div>
    </div>
  );
};
