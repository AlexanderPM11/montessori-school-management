import { useField } from 'formik';
import { IoChevronDown } from 'react-icons/io5'; // Importa el ícono

interface Option {
    id: string | number;
    text: string;
}

interface Props {
    label?: string;
    name: string;
    placeholder?: string;
    customClassName?: string;
    px?: string;
    py?: string;
    options?: Option[] | null;
    onChangeCustom?: (value: string) => void;
    [x: string]: unknown;
}

export const CustomSelect = ({ label, px, py, options, onChangeCustom, customClassName, ...props }: Props) => {
    const [field, meta] = useField(props);

    const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        // Lógica del campo (Formik).
        field.onChange(event);
        if (onChangeCustom) {
            onChangeCustom(value); // Ejecuta la función personalizada.
        }
    };

    return (
        <div className={customClassName}>
            <label className='text-gray-900'>
                {label}
                <div className="relative">
                    <select
                        className={`w-full 
                            ${label ? 'mt-1' : ''} 
                            ${px ? `px-${px}` : 'px-2'} 
                            ${py ? `py-${py}` : 'py-4'} 
                            pr-8 
                            text-gray-900
                            rounded-lg bg-white border border-gray-200 text-sm focus:outline-none 
                            appearance-none
                            ${meta.touched && meta.error ? 'border-red-400' : ''}`}
                        {...field}
                        {...props}
                        onChange={handleChange}
                    >
                        <option value="">
                            {props.placeholder || 'Sin seleccionar'}
                        </option>
                        {options &&
                            options.map((option) => (
                                <option key={option.id} value={option.id}>
                                    {option.text}
                                </option>
                            )
                            )}
                    </select>
                    {/* Ícono personalizado */}
                    <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none">
                        <IoChevronDown className="text-gray-900" />
                    </div>
                </div>
            </label>
            {meta.touched && meta.error ? (
                <div className="text-red-500">{meta.error}</div>
            ) : null}
        </div>
    );
};