// Funcionalidades JavaScript para o sistema NFSe

// Função para download de arquivos
window.downloadFile = (filename, content, contentType) => {
    const a = document.createElement('a');
    const file = new Blob([content], { type: contentType });

    a.href = URL.createObjectURL(file);
    a.download = filename;
    a.click();

    URL.revokeObjectURL(a.href);
};

// Função para formatação de CNPJ
window.maskCNPJ = (value) => {
    if (!value) return value;

    // Remove tudo que não é dígito
    value = value.replace(/\D/g, '');

    // Aplica máscara 00.000.000/0000-00
    if (value.length <= 2) return value;
    if (value.length <= 5) return value.replace(/(\d{2})(\d{0,3})/, '$1.$2');
    if (value.length <= 8) return value.replace(/(\d{2})(\d{3})(\d{0,3})/, '$1.$2.$3');
    if (value.length <= 12) return value.replace(/(\d{2})(\d{3})(\d{3})(\d{0,4})/, '$1.$2.$3/$4');
    return value.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{0,2})/, '$1.$2.$3/$4-$5');
};

// Função para formatação de CPF
window.maskCPF = (value) => {
    if (!value) return value;

    // Remove tudo que não é dígito
    value = value.replace(/\D/g, '');

    // Aplica máscara 000.000.000-00
    if (value.length <= 3) return value;
    if (value.length <= 6) return value.replace(/(\d{3})(\d{0,3})/, '$1.$2');
    if (value.length <= 9) return value.replace(/(\d{3})(\d{3})(\d{0,3})/, '$1.$2.$3');
    return value.replace(/(\d{3})(\d{3})(\d{3})(\d{0,2})/, '$1.$2.$3-$4');
};

// Função para formatação de CEP
window.maskCEP = (value) => {
    if (!value) return value;

    // Remove tudo que não é dígito
    value = value.replace(/\D/g, '');

    // Aplica máscara 00000-000
    if (value.length <= 5) return value;
    return value.replace(/(\d{5})(\d{0,3})/, '$1-$2');
};

// Função para formatação de telefone
window.maskPhone = (value) => {
    if (!value) return value;

    // Remove tudo que não é dígito
    value = value.replace(/\D/g, '');

    // Aplica máscara (00) 00000-0000 ou (00) 0000-0000
    if (value.length <= 2) return value;
    if (value.length <= 6) return value.replace(/(\d{2})(\d{0,4})/, '($1) $2');
    if (value.length <= 10) return value.replace(/(\d{2})(\d{4})(\d{0,4})/, '($1) $2-$3');
    return value.replace(/(\d{2})(\d{5})(\d{0,4})/, '($1) $2-$3');
};

// Função para validação de CNPJ
window.validateCNPJ = (cnpj) => {
    if (!cnpj) return false;

    // Remove caracteres não numéricos
    cnpj = cnpj.replace(/\D/g, '');

    // Verifica se tem 14 dígitos
    if (cnpj.length !== 14) return false;

    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1+$/.test(cnpj)) return false;

    // Validação do primeiro dígito verificador
    let soma = 0;
    let peso = 2;
    for (let i = 11; i >= 0; i--) {
        soma += parseInt(cnpj.charAt(i)) * peso;
        peso = peso === 9 ? 2 : peso + 1;
    }

    let resto = soma % 11;
    let digito1 = resto < 2 ? 0 : 11 - resto;

    if (parseInt(cnpj.charAt(12)) !== digito1) return false;

    // Validação do segundo dígito verificador
    soma = 0;
    peso = 2;
    for (let i = 12; i >= 0; i--) {
        soma += parseInt(cnpj.charAt(i)) * peso;
        peso = peso === 9 ? 2 : peso + 1;
    }

    resto = soma % 11;
    let digito2 = resto < 2 ? 0 : 11 - resto;

    return parseInt(cnpj.charAt(13)) === digito2;
};

// Função para validação de CPF
window.validateCPF = (cpf) => {
    if (!cpf) return false;

    // Remove caracteres não numéricos
    cpf = cpf.replace(/\D/g, '');

    // Verifica se tem 11 dígitos
    if (cpf.length !== 11) return false;

    // Verifica se todos os dígitos são iguais
    if (/^(\d)\1+$/.test(cpf)) return false;

    // Validação do primeiro dígito verificador
    let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseInt(cpf.charAt(i)) * (10 - i);
    }

    let resto = soma % 11;
    let digito1 = resto < 2 ? 0 : 11 - resto;

    if (parseInt(cpf.charAt(9)) !== digito1) return false;

    // Validação do segundo dígito verificador
    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpf.charAt(i)) * (11 - i);
    }

    resto = soma % 11;
    let digito2 = resto < 2 ? 0 : 11 - resto;

    return parseInt(cpf.charAt(10)) === digito2;
};

// Função para buscar CEP via API
window.buscarCEP = async (cep) => {
    try {
        // Remove caracteres não numéricos
        cep = cep.replace(/\D/g, '');

        if (cep.length !== 8) {
            throw new Error('CEP deve ter 8 dígitos');
        }

        const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
        const data = await response.json();

        if (data.erro) {
            throw new Error('CEP não encontrado');
        }

        return {
            cep: data.cep,
            logradouro: data.logradouro,
            complemento: data.complemento,
            bairro: data.bairro,
            localidade: data.localidade,
            uf: data.uf,
            ibge: data.ibge,
            gia: data.gia,
            ddd: data.ddd,
            siafi: data.siafi
        };
    } catch (error) {
        console.error('Erro ao buscar CEP:', error);
        throw error;
    }
};

// Função para highlight de XML (básico)
window.highlightXml = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element) return;

    let content = element.textContent || element.innerText;

    // Highlight básico para XML
    content = content
        .replace(/(&lt;[^&gt;]+&gt;)/g, '<span class="xml-tag">$1</span>')
        .replace(/(&quot;[^&quot;]*&quot;)/g, '<span class="xml-attr">$1</span>')
        .replace(/(xmlns[^=]*=)/g, '<span class="xml-namespace">$1</span>');

    element.innerHTML = content;
};

// Função para copiar texto para clipboard
window.copyToClipboard = async (text) => {
    try {
        if (navigator.clipboard) {
            await navigator.clipboard.writeText(text);
            return true;
        } else {
            // Fallback para navegadores mais antigos
            const textArea = document.createElement('textarea');
            textArea.value = text;
            document.body.appendChild(textArea);
            textArea.select();
            document.execCommand('copy');
            document.body.removeChild(textArea);
            return true;
        }
    } catch (error) {
        console.error('Erro ao copiar para clipboard:', error);
        return false;
    }
};

// Função para abrir modal de confirmação
window.confirmAction = (message) => {
    return confirm(message);
};

// Função para salvar no localStorage
window.saveToStorage = (key, value) => {
    try {
        localStorage.setItem(key, JSON.stringify(value));
        return true;
    } catch (error) {
        console.error('Erro ao salvar no localStorage:', error);
        return false;
    }
};

// Função para carregar do localStorage
window.loadFromStorage = (key) => {
    try {
        const item = localStorage.getItem(key);
        return item ? JSON.parse(item) : null;
    } catch (error) {
        console.error('Erro ao carregar do localStorage:', error);
        return null;
    }
};

// Função para validar e-mail
window.validateEmail = (email) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
};

// Função para formatar moeda (Real)
window.formatCurrency = (value) => {
    if (!value) return 'R$ 0,00';

    const number = parseFloat(value);
    return new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
    }).format(number);
};

// Função para parsear moeda para número
window.parseCurrency = (value) => {
    if (!value) return 0;

    // Remove tudo exceto números, vírgula e ponto
    const cleaned = value.toString()
        .replace(/[^\d,.-]/g, '')
        .replace(',', '.');

    return parseFloat(cleaned) || 0;
};

// Função para validar valores monetários
window.validateCurrency = (value, min = 0, max = null) => {
    const number = parseCurrency(value);

    if (isNaN(number) || number < min) return false;
    if (max !== null && number > max) return false;

    return true;
};

// CSS para highlight de XML
const xmlCss = `
    .xml-tag { color: #0066cc; font-weight: bold; }
    .xml-attr { color: #cc6600; }
    .xml-namespace { color: #cc0066; }
    .xml-content { font-family: 'Courier New', monospace; }
`;

// Adiciona CSS ao documento
const style = document.createElement('style');
style.textContent = xmlCss;
document.head.appendChild(style);

// Função para debug (remover em produção)
window.nfseDebug = {
    log: (message, data = null) => {
        if (console && console.log) {
            console.log('[NFSe Debug]', message, data);
        }
    },
    error: (message, error = null) => {
        if (console && console.error) {
            console.error('[NFSe Error]', message, error);
        }
    }
};

// Inicialização quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', function () {
    console.log('NFSe System JavaScript loaded successfully');

    // Configurações globais
    window.nfseConfig = {
        version: '1.0.0',
        environment: 'development', // ou 'production'
        apiBaseUrl: '/api',
        maxFileSize: 5 * 1024 * 1024, // 5MB
        allowedFileTypes: ['.xml', '.txt'],
        autoSave: true,
        autoSaveInterval: 30000 // 30 segundos
    };
});