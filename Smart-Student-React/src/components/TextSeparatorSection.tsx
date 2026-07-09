
interface Props {
    customStyle?: string;
    text: string;
}



export const TextSeparatorSection = ({ text }: Props) => {
    return (
        <h1 className="text-[20px] font-bold text-gray-900 mb-">
            {text}
        </h1>
    )
}