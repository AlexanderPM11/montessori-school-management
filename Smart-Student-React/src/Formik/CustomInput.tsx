import { useState, useEffect } from "react";
import { useField } from "formik";
import { FiEye, FiEyeOff } from "react-icons/fi";
import { formatTel } from "../util/generalFormattedField";

interface Props {
  label?: string;
  requerid?: boolean;
  name: string;
  placeholder: string;
  customClassName?: string;
  px?: string;
  py?: string;
  type: "text" | "email" | "password" | "date" | "textarea" | "tel";
  pattern?: string;
  rows?: number;
  value?: string; // <--- Añadido
  [x: string]: unknown;
  onChangeCustom?: (value: string, previousValue: string) => string;
  onChangeCustomVoid?: (value: string) => void;
  style?: React.CSSProperties;
}

export const CustomInput = ({
  label,
  px,
  py,
  onChangeCustom,
  onChangeCustomVoid,
  customClassName,
  type,
  requerid,
  pattern,
  rows = 3,
  value, // <--- Recibir valor externo
  style,
  ...props
}: Props) => {
  const [field, meta, helpers] = useField(props);
  const [showPassword, setShowPassword] = useState(false);
  const [previousValue, setPreviousValue] = useState<string>("");

  // Efecto para formatear el valor inicial si es un teléfono
  useEffect(() => {
    if (type === "tel" && (value ?? field.value)) {
      const formatted = formatTel(value ?? field.value, "");
      helpers.setValue(formatted);
      setPreviousValue(formatted);
    }
  }, [type]);

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    let newValue = event.target.value;

    // Formato de teléfono
    if (type === "tel") {
      newValue = formatTel(newValue, previousValue);
      setPreviousValue(newValue);
      helpers.setValue(newValue);
      return;
    }

    if (onChangeCustom) {
      const result = onChangeCustom(newValue, previousValue);
      newValue = result;
      setPreviousValue(result);
    }

    if (onChangeCustomVoid) {
      onChangeCustomVoid(newValue);
    }

    helpers.setValue(newValue);
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className={`custom-input-container ${customClassName}`} style={style}>
      <label className="text-gray-900">
        {label} {requerid && <span className="text-red-500 font-bold">*</span>}
        <div className="relative">
          {type === "textarea" ? (
            <textarea
              className={`w-full ${label ? "mt-1" : ""} ${
                px ? `px-${px}` : "px-4"
              } ${py ? `py-${py}` : "py-4"}
              rounded-lg bg-white border border-gray-200 text-sm focus:outline-none input-field
              ${meta.touched && meta.error ? "border-red-400" : ""}`}
              {...field}
              {...props}
              rows={rows}
              onChange={handleChange}
              value={value ?? field.value} // <--- Prioriza el value externo
            />
          ) : (
            <input
              className={`w-full ${label ? "mt-1" : ""} ${
                px ? `px-${px}` : "px-4"
              } ${py ? `py-${py}` : "py-4"}
              rounded-lg bg-white border border-gray-200 text-sm focus:outline-none input-field
              ${meta.touched && meta.error ? "border-red-400" : ""}`}
              {...field}
              {...props}
              type={type === "password" && showPassword ? "text" : type}
              onChange={handleChange}
              pattern={pattern}
              value={value ?? field.value} // <--- Prioriza el value externo
            />
          )}
          {type === "password" && (
            <span
              className="absolute right-[1rem] top-1/2 transform -translate-y-1/2 cursor-pointer"
              onClick={togglePasswordVisibility}
            >
              {showPassword ? <FiEyeOff /> : <FiEye />}
            </span>
          )}
        </div>
      </label>
      {meta.touched && meta.error ? (
        <div className="text-red-500">{meta.error}</div>
      ) : null}
    </div>
  );
};
