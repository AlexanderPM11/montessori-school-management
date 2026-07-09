import React, { useState, useEffect } from 'react';
import { FaArrowDown } from 'react-icons/fa'; // Necesitas instalar react-icons

export const ScrollToBottomButton: React.FC = () => {
    const [isVisible, setIsVisible] = useState(false);

    // Mostrar el botón si el usuario no está al final de la página
    useEffect(() => {
        const toggleVisibility = () => {
            const scrollPosition = window.scrollY + window.innerHeight;
            const pageHeight = document.documentElement.scrollHeight;

            // Mostrar el botón si el usuario no está cerca del final de la página
            if (scrollPosition < pageHeight - 200) {
                setIsVisible(true);
            } else {
                setIsVisible(false);
            }
        };

        window.addEventListener('scroll', toggleVisibility);
        return () => window.removeEventListener('scroll', toggleVisibility);
    }, []);

    const scrollToBottom = () => {
        window.scrollTo({
            top: document.documentElement.scrollHeight, // Lleva al final de la página
            behavior: 'smooth', // Scroll suave hacia abajo
        });
    };

    return (
        isVisible && (
            <button
                onClick={scrollToBottom}
                className="fixed bottom-4 right-4 w-12 h-12 bg-gray-900 text-white rounded-full shadow-lg flex items-center justify-center hover:bg-gray-800 transition duration-300"
                aria-label="Scroll to bottom"
            >
                <FaArrowDown size={20} />
            </button>
        )
    );
};