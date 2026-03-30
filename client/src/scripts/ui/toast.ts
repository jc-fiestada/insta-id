export type ToastType = 'success' | 'error' | 'warning' | 'info';

export interface ToastOptions {
  type?: ToastType;
  title?: string;
  message: string;
  duration?: number; // ms, 0 = persistent
}

const ICONS: Record<ToastType, string> = {
  success: '✓',
  error:   '✕',
  warning: '⚠',
  info:    'ℹ',
};

const TITLES: Record<ToastType, string> = {
  success: 'Success',
  error:   'Error',
  warning: 'Warning',
  info:    'Info',
};

function getContainer(): HTMLElement {
  const existing = document.getElementById('toast-container');
  if (existing) return existing;

  const container = document.createElement('div');
  container.id = 'toast-container';
  container.className = 'toast-container';
  container.setAttribute('aria-live', 'polite');
  container.setAttribute('aria-label', 'Notifications');
  document.body.appendChild(container);
  return container;
}

export function showToast(options: ToastOptions): void {
  const {
    type = 'info',
    title = TITLES[type],
    message,
    duration = 4000,
  } = options;

  const container = getContainer();

  const toast = document.createElement('div');
  toast.className = `toast toast--${type}`;
  toast.setAttribute('role', 'alert');
  toast.innerHTML = `
    <span class="toast__icon" aria-hidden="true">${ICONS[type]}</span>
    <div class="toast__body">
      <span class="toast__title">${title}</span>
      <p class="toast__message">${message}</p>
    </div>
    <button class="toast__close" aria-label="Dismiss notification">✕</button>
  `;

  container.appendChild(toast);

  requestAnimationFrame(() => {
    requestAnimationFrame(() => {
      toast.classList.add('toast--visible');
    });
  });

  const dismiss = () => {
    toast.classList.add('toast--hiding');
    toast.classList.remove('toast--visible');
    toast.addEventListener('transitionend', () => toast.remove(), { once: true });
  };

  toast.querySelector('.toast__close')?.addEventListener('click', dismiss);

  if (duration > 0) {
    setTimeout(dismiss, duration);
  }
}