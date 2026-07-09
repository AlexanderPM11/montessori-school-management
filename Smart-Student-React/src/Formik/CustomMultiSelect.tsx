/* eslint-disable @typescript-eslint/no-explicit-any */
import { useField } from "formik";
import Select, { MultiValue, SingleValue } from "react-select";
import { SelectedInterface } from "../interfaces/SelectedInterface";

interface Props {
  label?: string;
  requerid?: boolean;
  name: string;
  options?: SelectedInterface<unknown>[] | null;
  customClassName?: string;
  onChangeCustom?: (values: unknown) => void;
  isMulti?: boolean;
}

export const CustomMultiSelect = ({
  label,
  customClassName,
  options,
  requerid,
  onChangeCustom,
  isMulti = false,
  ...props
}: Props) => {
  const [field, meta, helpers] = useField(props);

  const transformedOptions = options?.map((option) => ({
    value: (option.id as string | number).toString(),
    label: option.text.toString(),
  }));

  const handleChange = (
    newValue: MultiValue<unknown> | SingleValue<unknown>
  ) => {
    let values;

    if (newValue) {
      if (isMulti) {
        values = (newValue as { value: string | number }[]).map((option) =>
          option.value.toString()
        );
      } else {
        values = (newValue as { value: string | number }).value.toString();
      }
    } else {
      values = isMulti ? [] : "";
    }

    helpers.setValue(values);

    if (onChangeCustom) {
      onChangeCustom(values);
    }
  };

  const selectedOptions = isMulti
    ? transformedOptions?.filter((option) =>
        (field.value ?? [])
          .map((v: string | number) => v.toString())
          .includes(option.value)
      ) ?? []
    : transformedOptions?.find(
        (option) => option.value === field.value?.toString()
      ) ?? null;

  const customStyles = {
    control: (baseStyles: any) => ({
      ...baseStyles,
      marginTop: "0.25rem",
      paddingLeft: "2px",
      paddingRight: "2px",
      paddingTop: "9px",
      paddingBottom: "9px",
      borderRadius: "0.5rem",
      backgroundColor: "white",
      borderColor: meta.touched && meta.error ? "red" : "#E5E7EB",
      borderWidth: "1px",
      borderStyle: "solid",
      textAlign: "left",
      fontSize: "0.875rem",
      boxShadow: "none",
      outline: "none",
      ":hover": {
        borderColor: meta.touched && meta.error ? "red" : "#E5E7EB",
      },
      appearance: "none",
    }),
  };

  return (
    <div className={customClassName}>
      {label && (
        <label className="text-gray-900">
          {label}
          {requerid && <span className="text-red-500 font-bold"> *</span>}
        </label>
      )}

      <Select
        options={transformedOptions ?? []}
        isMulti={isMulti}
        placeholder="Selecciona una opción"
        value={selectedOptions}
        onChange={handleChange}
        {...props}
        styles={customStyles}
      />

      {meta.touched && meta.error ? (
        <div className="text-red-500">{meta.error}</div>
      ) : null}
    </div>
  );
};
