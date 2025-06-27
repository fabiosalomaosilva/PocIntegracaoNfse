// Função para download de arquivos
window.downloadFileFromBase64 = (base64, fileName, contentType) => {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${base64}`;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

// Função para syntax highlighting básico
window.highlightXml = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element) return;

    let xml = element.textContent;

    // Highlight tags
    xml = xml.replace(/(&lt;\/?[^&gt;]+&gt;)/g, '<span class="xml-tag">$1</span>');

    // Highlight attributes
    xml = xml.replace(/(\w+)=("[^"]*")/g, '<span class="xml-attribute">$1</span>=<span class="xml-value">$2</span>');

    // Highlight comments
    xml = xml.replace(/(&lt;!--.*?--&gt;)/g, '<span class="xml-comment">$1</span>');

    element.innerHTML = xml;
};

// Função para formatar XML
window.formatXml = (xml) => {
    const PADDING = ' '.repeat(2);
    const reg = /(>)(<)(\/*)/g;
    let formatted = '';
    let pad = 0;

    xml = xml.replace(reg, '$1\r\n$2$3');

    xml.split('\r\n').forEach((node) => {
        let indent = 0;
        if (node.match(/.+<\/\w[^>]*>$/)) {
            indent = 0;
        } else if (node.match(/^<\/\w/) && pad > 0) {
            pad -= 1;
        } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
            indent = 1;
        } else {
            indent = 0;
        }

        formatted += PADDING.repeat(pad) + node + '\r\n';
        pad += indent;
    });

    return formatted;
};

// Função para validação de CNPJ
window.validateCNPJ = (cnpj) => {
    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj.length !== 14) return false;

    if (/^(\d)\1+$/.test(cnpj)) return false;

    let tamanho = cnpj.length - 2;
    let numeros = cnpj.substring(0, tamanho);
    let digitos = cnpj.substring(tamanho);
    let soma = 0;
    let pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }

    let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0)) return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1)) return false;

    return true;
};

// Função para máscara de CNPJ
window.maskCNPJ = (value) => {
    return value
        .replace(/\D/g, '')
        .replace(/^(\d{2})(\d)/, '$1.$2')
        .replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3')
        .replace(/\.(\d{3})(\d)/, '.$1/$2')
        .replace(/(\d{4})(\d)/, '$1-$2')
        .slice(0, 18);
};

// Função para máscara de CPF
window.maskCPF = (value) => {
    return value
        .replace(/\D/g, '')
        .replace(/^(\d{3})(\d)/, '$1.$2')
        .replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3')
        .replace(/\.(\d{3})(\d)/, '.$1-$2')
        .slice(0, 14);
};

// Auto-save functionality
let autoSaveTimeout;
window.setupAutoSave = (dotNetHelper, interval = 30000) => {
    clearTimeout(autoSaveTimeout);
    autoSaveTimeout = setTimeout(() => {
        dotNetHelper.invokeMethodAsync('AutoSave');
    }, interval);
};
