using System;

namespace Core.Services {
    public class ScopeService {
        public Guid ScopeId { get; }

        public ScopeService() {
            ScopeId = Guid.NewGuid();
        }
    }
}
